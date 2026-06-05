using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class OptionKeyController : MonoBehaviour
{
    [Header("参照")]
    [SerializeField]
    private VolumeOption _volumeOption;
    [SerializeField]
    private NotesOption _notesOption;
    [SerializeField]
    private TimingOption _timingOption;
    [SerializeField]
    private EffectOption _effectOption;
    [SerializeField]
    private JudgeDisplayOption _judgeDPOption;

    [Header("音量スライダー参照")]
    [SerializeField]
    private Slider _masterSlider;
    [SerializeField]
    private Slider _bgmSlider;
    [SerializeField]
    private Slider _seSlider;
    [SerializeField]
    private Slider _musicSlider;


    private SettingItem _currentItem;
   

    enum SettingItem
    {
        MASTER,
        BGM,
        SE,
        MUSIC,
        NOTES_SPEED,
        TIMING,
        EFFECT,
        JUDGE
    }

    int MaxIndex => System.Enum.GetValues(typeof(SettingItem)).Length - 1;

    // Update is called once per frame
    void Update()
    {
        HandleSelect();
        HandleAdjust();
    }

    void HandleSelect()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlaySelectSE();
            _currentItem--;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlaySelectSE();
            _currentItem++;
        }

        _currentItem = (SettingItem)Mathf.Clamp((int)_currentItem, 0, MaxIndex);
    }

    void HandleAdjust()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Adjust(-1);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Adjust(1);
        }
    }

    void Adjust(int dir)
    {
        float step = 1.0f;

        switch(_currentItem)
        {
            case SettingItem.MASTER:
                AddSliderValue(_masterSlider, dir);
                break;
            case SettingItem.BGM:
                AddSliderValue(_bgmSlider, dir);
                break;
            case SettingItem.SE:
                AddSliderValue(_seSlider, dir);
                break;
            case SettingItem.MUSIC:
                AddSliderValue(_musicSlider, dir);
                break;
            case SettingItem.NOTES_SPEED:
                ChangeOption(dir, _notesOption.OnClickDecrease, _notesOption.OnClickIncrease);
                break;
            case SettingItem.TIMING:
                ChangeOption(dir, _timingOption.OnClickDecrease, _timingOption.OnClickIncrease);
                break;
            case SettingItem.EFFECT:
                _effectOption.SetState(dir > 0);
                break;
            case SettingItem.JUDGE:
                _judgeDPOption.SetState(dir > 0);
                break;
        }
    }

    //---共通処理---
    void AddSliderValue(Slider slider)
    {
        AddSliderValue(slider, 1);
    }

    void AddSliderValue(Slider slider,int dir)
    {
        if (slider == null) return;

        float step = slider.wholeNumbers ? 1.0f : 0.1f;
        slider.value = Mathf.Clamp(
            slider.value + dir * step,
            slider.minValue,
            slider.maxValue
            );

    }

    void ChangeOption(int dir,System.Action decrease,System.Action increase)
    {
        if (dir < 0) decrease?.Invoke();
        else increase?.Invoke();
    }

    void PlaySelectSE()
    {
        SoundEffectManager.instance.SetSoundEffectState(SoundEffectType.SELECT);
    }

}
