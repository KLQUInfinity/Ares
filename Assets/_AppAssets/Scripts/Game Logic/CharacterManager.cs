using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    public List<Character> characters = new List<Character>();
    [SerializeField]
    public List<CharacterLevel> charactersLevels;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        //GameObject obj = GameObject.Find("Character)");
        //Character character = new Character(obj);
        //characters.Add(character);
        foreach (var character in characters)
        {
            character.characterLevel = charactersLevels[0]; //Inital values need to be calculated.
            character.characterLevels.Add(charactersLevels[0]);
            character.Init();
        }
    }

    public Character getCharacterWithGameObject(GameObject characterGameObject)
    {

        foreach (var character in characters)
        {
            if (character == characterGameObject)
            {
                return character;
            }
        }
        return null;
    }

    public void OnCharacterPopulation(Room room, Character character)
    {
        if (room.roomGameObject.name != "HibernationRoom")
        {
            Job job = room.getRandomVacantJob(character);
            if (job != null)
            {
                character.assignJob(job);
                character.job.assignJobHolder(character);
            }
            else
            {
                character.deassignJob();
            }
            character.container = room.roomGameObject;
        }

    }

    public void OnCharacterEvacuation(Character character)
    {
        if (character.job != null)
        {
            character.job.deassignJobHolder();
            character.deassignJob();
            character.characterGameObject.GetComponent<CharacterEntity>().characterAnimationFSM.resetOriginalAnimationClips();
        }
    }

    public void levelCharacterUp(Character character)
    {
        character.characterLevel = charactersLevels[charactersLevels.IndexOf(character.characterLevel) + 1]; //Inital values need to be calculated.
        character.characterLevels.Add(character.characterLevel);
        character.characterLevel.totalLevelDaysWorkedHours[GameBrain.Instance.timeManager.gameTime.gameDay] = new GameTime();
    }

    public void addNewCharacter(Character character)
    {
        characters.Add(character);
        character.Init();
        Init();
    }

    #region Time related methods
    public void OnSecondChange()
    {//Called each real second
        foreach (var character in characters)
        {

            character.calculateCharacterHappiness();
            character.calculateProductivity();
            character.updateStamina();
        }

    }

    public void OnMinuteChange()
    {//Called each real Minute

    }

    public void OnGameHourChange()
    {//Called each Game Hour
        foreach (var character in characters)
        {
            character.updateCurrentCharacterLevelGameTimeDay();
        }
    }

    public void OnGameDayChange()
    {// Called each Game Day
        foreach (var character in characters)
        {
            character.updateCurrentCharacterLevelGameTimeDay();
            character.updateCharacterLevel();
        }
    }
    #endregion
}
