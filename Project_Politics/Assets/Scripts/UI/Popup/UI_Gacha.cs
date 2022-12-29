using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gacha : UI_Popup
{
    enum Objects
    {
        EnvelopImage,
        Back,
        GachaButton,
        Resume,
        White,
    }

    void Start()
    {
        Bind<GameObject>(typeof(Objects));

        BindEvent(GetObject((int)Objects.Back), (PointerEventData) => Managers.UI.ClosePopup(this));
        BindEvent(GetObject((int)Objects.GachaButton), (PointerEventData) => StartCoroutine(Gacha()));
        GetObject((int)Objects.Resume).SetActive(false);
    }

    private IEnumerator Gacha()
    {
        SetResume();
        GetObject((int)Objects.Resume).SetActive(false);
        GetObject((int)Objects.GachaButton).SetActive(false);
        GetObject((int)Objects.Back).SetActive(false);
        GetObject((int)Objects.EnvelopImage).SetActive(true);
        GetObject((int)Objects.White).SetActive(true);
        GetComponent<Animator>().SetTrigger("Open");

        yield return new WaitForSeconds(2f);
        GetObject((int)Objects.EnvelopImage).SetActive(false);
        GetObject((int)Objects.White).SetActive(false);
        GetObject((int)Objects.Resume).SetActive(true);
        GetComponent<Animator>().SetTrigger("Exit");

        yield return new WaitForSeconds(0.2f);
        GetObject((int)Objects.Back).SetActive(true);
        GetObject((int)Objects.GachaButton).SetActive(true);
    }

    private Secretary GetNewSecretary()
    {
        return Managers.Data.Secretarys[UnityEngine.Random.Range(0, Managers.Data.Secretarys.Count)];
    }

    private void SetResume()
    {
        Secretary newSec = GetNewSecretary();
        Managers.Data.GameData.AddSecretary(newSec);
        GameObject resume = GetObject((int)Objects.Resume);
        GameObject image = Util.FindChild(resume, "Image");
        image.GetComponent<Animator>().runtimeAnimatorController = newSec.Motion;
        image.GetComponent<Image>().sprite = image.GetComponent<SpriteRenderer>().sprite;
        Util.FindChild(resume, "Name").GetComponent<Text>().text = newSec.Name;
    }
}
