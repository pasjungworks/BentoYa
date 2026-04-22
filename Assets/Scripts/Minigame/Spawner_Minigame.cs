using System.Collections.Generic;
using UnityEngine;

public class Spawner_Minigame : MonoBehaviour
{
    public List<GameObject> _sprite = new List<GameObject>();
    private List<int> spriteBag = new List<int>();

    public Transform noteContainer;

    private GameObject _spawnNote;

    public float notelifeTime;

    public Vector3 sizeNote;

    public float delaySpawn;
    private void Start()
    {
        Reset_Sprite();
    }
    public void SpawnRandom()
    {
        if (spriteBag.Count == 0)
            Reset_Sprite();

        int spriteIndex = Random.Range(0, spriteBag.Count);
        int randomSprite = spriteBag[spriteIndex];

        _spawnNote = Instantiate(_sprite[randomSprite], transform.position, Quaternion.identity);

        spriteBag.RemoveAt(spriteIndex);

        Destroy( _spawnNote, notelifeTime);
    }
    public void Reset_Sprite()
    {
        for (int i = 0; i < _sprite.Count; i++)
            spriteBag.Add(i);
    }


    public GameObject SpawnByIndex(int index)
    {
        if (index < 0 || index >= _sprite.Count) return null;

        GameObject spawnedNote = Instantiate(_sprite[index], noteContainer);

        spawnedNote.transform.SetParent(noteContainer, false);
        spawnedNote.transform.SetAsLastSibling();

        Vector3 parentScale = noteContainer.localScale;

        RectTransform rect = spawnedNote.GetComponent<RectTransform>();
        if (rect != null)
        {

            rect.localPosition = Vector3.zero;
            rect.localScale = new Vector3(sizeNote.x / parentScale.x, sizeNote.y / parentScale.y, sizeNote.z / parentScale.z);


            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 10);
        }

        Destroy(spawnedNote, notelifeTime);
        return spawnedNote;
    }
}
