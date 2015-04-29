using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiTutorial : MonoBehaviour {

    public Image buttonJump;
    public Image buttonShoot;
    public Image helpSprite;

    public Image helpJumpSpriteKeyboard;
    public Image helpShootSpriteKeyboard;

    public Image DoubleJumpHelpSprite;
    public Image ShootHelpSprite;

    private bool ready;
    private int jumpLevel = 2;
    private int doubleJumpLevel = 3;
    private int shootLevel = 4;

    public states state;
    public enum states
    {
        HIDE,
        JUMP,
        DOUBLEJUMP,
        SHOOT,
        READY
    }

    private CharactersManager charactersManager;

    private bool isAndroid()
    {
        if (Application.platform == RuntimePlatform.Android)
            return true;
        return false;
    }
	void Start () 
    {
        DoubleJumpHelpSprite.enabled = false;
        helpSprite.enabled = false;
        buttonJump.enabled = false;
        buttonShoot.enabled = false;
        helpJumpSpriteKeyboard.enabled = false;

        ShootHelpSprite.enabled = false;
        helpShootSpriteKeyboard.enabled = false;
        
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarJump += OnAvatarJump;
        Data.Instance.events.OnAvatarShoot += OnAvatarShoot;

        charactersManager = Game.Instance.GetComponent<CharactersManager>();
	}
    private bool canDisplaySignal()
    {
        if (charactersManager.getMainCharacter() == null
            ||
            charactersManager.getMainCharacter().state == CharacterBehavior.states.CRASH
            ||
           charactersManager.getMainCharacter().state == CharacterBehavior.states.DEAD
        )
            return false;
        return true;
    }
    private void OnListenerDispatcher(string message)
    {
        if (!canDisplaySignal())
            return;
        
        if (Data.Instance.missionActive == 1)
            state = states.HIDE;
        else if (Data.Instance.missionActive == jumpLevel)
            state = states.JUMP;
        else if (Data.Instance.missionActive == doubleJumpLevel) 
            state = states.DOUBLEJUMP;
        else if (Data.Instance.missionActive == shootLevel)
            state = states.SHOOT;
        else
            state = states.READY;

        if (state == states.READY && message == "ShowMissionId")
            Invoke("setOn", 2);
        else if (state != states.HIDE && message == "ShowMissionId")
            if(state == states.SHOOT)
                Invoke("setOn", 3.5f);
            else
                Invoke("setOn", 2.2f);
        else if (state != states.READY && state != states.HIDE && message == "ShowMissionName")
            showHelp();
    }
    private void showHelp()
    {
        if (isAndroid())
        {
            if (state == states.JUMP)
                StartCoroutine(Play(helpSprite.GetComponent<Animation>(), "HelpSignalOn", false));
            else if (state == states.DOUBLEJUMP)
                StartCoroutine(Play(DoubleJumpHelpSprite.GetComponent<Animation>(), "HelpSignalOn", false));
            else
                StartCoroutine(Play(ShootHelpSprite.GetComponent<Animation>(), "HelpSignalOn", false));
        }
        else
        {
            if (state == states.JUMP)
                StartCoroutine(Play(helpJumpSpriteKeyboard.GetComponent<Animation>(), "HelpSignalOn", false));
            else if (state == states.DOUBLEJUMP)
                StartCoroutine(Play(helpJumpSpriteKeyboard.GetComponent<Animation>(), "HelpSignalOn", false));
            else if (state == states.SHOOT)
                StartCoroutine(Play(helpShootSpriteKeyboard.GetComponent<Animation>(), "HelpSignalOn", false));
        }
    }
	public void setOn () 
    {
        if (!canDisplaySignal()) return;

        if (isAndroid())
        {
            if (state == states.JUMP || state == states.DOUBLEJUMP)
                animateButton(buttonJump);
            else 
            {
                animateButton(buttonJump);
                animateButton(buttonShoot);
                if (state == states.READY)
                    Reset();
            }
        }
	}
    void animateButton(Image sprite)
    {
        float originalY = sprite.transform.position.y;
        sprite.transform.position = new Vector3(sprite.transform.position.x, originalY - 1f, 0);

        sprite.enabled = true;

        Hashtable tweenData = new Hashtable();
        tweenData.Add("y", originalY);
        tweenData.Add("time", 1.5f);
        tweenData.Add("easeType", iTween.EaseType.easeOutQuad);

        iTween.MoveTo(sprite.gameObject, tweenData);
    }
    void OnDestroy()
    {
        Reset();
    }
    void Reset()
    {
        Data.Instance.events.OnAvatarJump -= OnAvatarJump;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
    }
    private void OnAvatarJump()
    {
         if (!ready) return;
         if (state == states.SHOOT) return;
         ready = false;

         helpSprite.enabled = false;
         DoubleJumpHelpSprite.enabled = false;
         helpJumpSpriteKeyboard.enabled = false;

         Game.Instance.UnPause();
         //Data.Instance.events.OnAvatarJump -= OnAvatarJump;
    }
    private void OnAvatarShoot()
    {

        if (!ready) return;
        if (state != states.SHOOT) return;
        ready = false;

        ShootHelpSprite.enabled = false;
        helpShootSpriteKeyboard.enabled = false;

        Game.Instance.UnPause();
        //Data.Instance.events.OnAvatarJump -= OnAvatarJump;
    }
    

    //private IEnumerator Play(this Animation animation, string clipName, bool useTimeScale, Action onComplete)
    private IEnumerator Play(this Animation animation, string clipName, bool useTimeScale)
    {
        if (canDisplaySignal())
        {
            if (state == states.JUMP)
                yield return new WaitForSeconds(1.2f);
            else if (state == states.SHOOT)
                yield return new WaitForSeconds(1.8f);
            else if (state == states.DOUBLEJUMP)
            {
                yield return new WaitForSeconds(2.2f);
                charactersManager.getMainCharacter().state = CharacterBehavior.states.JUMP;
            }
        }
        if (canDisplaySignal())
        {

            ready = true;

            //if (ready) yield break;

            Game.Instance.Pause();

            if (isAndroid())
            {
                if (state == states.JUMP)
                    helpSprite.enabled = true;
                else if (state == states.DOUBLEJUMP)
                    DoubleJumpHelpSprite.enabled = true;
                else if (state == states.SHOOT)
                    ShootHelpSprite.enabled = true;
            }
            else if (state == states.JUMP)
                helpJumpSpriteKeyboard.enabled = true;
            else if (state == states.DOUBLEJUMP)
                helpJumpSpriteKeyboard.enabled = true;
            else if (state == states.SHOOT)
                helpShootSpriteKeyboard.enabled = true;

            //We Don't want to use timeScale, so we have to animate by frame..
            if (!useTimeScale)
            {
                AnimationState _currState = animation[clipName];
                bool isPlaying = true;
                float _progressTime = 0F;
                float _timeAtLastFrame = 0F;
                float _timeAtCurrentFrame = 0F;
                float deltaTime = 0F;


                animation.Play(clipName);

                _timeAtLastFrame = Time.realtimeSinceStartup;
                while (isPlaying)
                {
                    _timeAtCurrentFrame = Time.realtimeSinceStartup;
                    deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                    _timeAtLastFrame = _timeAtCurrentFrame;

                    _progressTime += deltaTime;
                    _currState.normalizedTime = _progressTime / _currState.length;
                    animation.Sample();

                    if (_progressTime >= _currState.length)
                    {
                        if (_currState.wrapMode != WrapMode.Loop)
                        {
                            isPlaying = false;
                        }
                        else
                        {
                            _progressTime = 0.0f;
                        }

                    }

                    yield return new WaitForEndOfFrame();
                }
                yield return null;
                //if (onComplete != null)
                //{
                //    onComplete();
                //}
            }
            else
                animation.Play(clipName);
        }
    }
}
