using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MP_Generator : MonoBehaviour
{

    [SerializeField]
    Text[] statObjs;
    [SerializeField]
    Dropdown[] statDrops;
    [SerializeField]
    GameObject resultsWindow, resetBut, resultsDex;
    [SerializeField]
    Text[] resultObjs;
    [SerializeField]
    Text[] resultAdvs;

    int[] atts, attLimits, social, skills;
    int HP, will;
    string[] ranks, natures;
    int[][] typesAdvs;
    int[] typeResults;

    string nature;
    int rank, typeOne, typeTwo;
    string[] types;

    // Start is called before the first frame update
    void Start()
    {
        atts = new int[5];
        attLimits = new int[5];
        social = new int[5];
        for(int i = 0; i < social.Length; i++)
        {
            social[i] = 1;
        }
        skills = new int[12];
        ranks = new string[5];
        natures = new string[26];
        types = new string[18];
        typesAdvs = new int[18][];
        for(int i = 0; i < 18; i++)
        {
            typesAdvs[i] = new int[18];
        }
        typeResults = new int[18];
        SetRanks();
        SetTypes();
        SetNatures();
        resultsWindow.SetActive(false);
        resetBut.SetActive(false);
        resultsDex.SetActive(false);
    }

    void GetResults()
    {

        int p = (attLimits[0] - atts[0]) + (attLimits[1] - atts[1]) + (attLimits[2] - atts[2]) + (attLimits[3] - atts[3]) + (attLimits[4] - atts[4]);
        

        int statPoints = rank * 2;
        if (statPoints != 0)
        {
            if (rank * 2 <= p)
            {
                int i = statPoints;

                while (i > 0)
                {
                    int attAdd = Random.Range(0, 5);
                    if (atts[attAdd] == attLimits[attAdd])
                        continue;
                    else
                    {
                        atts[attAdd]++;
                        i--;
                    }

                }
            }
            else
            {
                atts[0] = attLimits[0];
                atts[1] = attLimits[1];
                atts[2] = attLimits[2];
                atts[3] = attLimits[3];
                atts[4] = attLimits[4];
            }
            int x = rank * 2;
            while(x > 0)
            {
                int attAdd = Random.Range(0, 5);
                if (social[attAdd] == 10)
                    continue;
                else
                {
                    social[attAdd]++;
                    x--;
                }
            }
        }

        SetSkills();

        List<string> weak = new List<string>();
        List<string> resist = new List<string>();
        List<string> immune = new List<string>();

        for (int i = 0; i < 18; i++)
        {
            if (typeOne != typeTwo)
                typeResults[i] = typesAdvs[typeOne][i] + typesAdvs[typeTwo][i];
            else
                typeResults[i] = typesAdvs[typeOne][i];

            if (typeResults[i] == -2)
                weak.Add(types[i] + " x2");
            else if (typeResults[i] == -1)
                weak.Add(types[i]);
            else if (typeResults[i] == 1)
                resist.Add(types[i]);
            else if (typeResults[i] == 2)
                resist.Add(types[i] + " x2");
            else if (typeResults[i] > 2)
                immune.Add(types[i]);
        }
  
        resultAdvs[0].text = "Weak: " + System.String.Join(", ",weak);
        resultAdvs[1].text = "Resist: " + System.String.Join(", ", resist);
        resultAdvs[2].text = "Immune: " + System.String.Join(", ", immune);

        HP += atts[2];
        will = 2 + atts[4];

        nature = natures[Random.Range(0, 25)];

        SetResults();

        resultsWindow.SetActive(true);
        resetBut.SetActive(true);
        resultsDex.SetActive(true);
    }

    public void SubmitStats()
    {
        int firstCount = 0;
        for(int i = 0; i < 11; i++)
        {
            if (statObjs[i].text != "")
                firstCount++;
        }
        if(firstCount == 11)
        {
            atts[0] = System.Convert.ToInt32(statObjs[0].text);
            attLimits[0] = System.Convert.ToInt32(statObjs[1].text);
            atts[1] = System.Convert.ToInt32(statObjs[2].text);
            attLimits[1] = System.Convert.ToInt32(statObjs[3].text);
            atts[2] = System.Convert.ToInt32(statObjs[4].text);
            attLimits[2] = System.Convert.ToInt32(statObjs[5].text);
            atts[3] = System.Convert.ToInt32(statObjs[6].text);
            attLimits[3] = System.Convert.ToInt32(statObjs[7].text);
            atts[4] = System.Convert.ToInt32(statObjs[8].text);
            attLimits[4] = System.Convert.ToInt32(statObjs[9].text);

            HP = System.Convert.ToInt32(statObjs[10].text);

            typeOne = statDrops[0].value;
            typeTwo = statDrops[1].value;
            rank = statDrops[2].value;


            int secondCount = 0;
            for (int i = 0; i < 11; i++)
            {
                if (System.Convert.ToInt32(statObjs[i].text) > 0)
                    secondCount++;
            }
            if (secondCount == 11)
            {
                int count = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (atts[i] <= attLimits[i])
                        count++;
                }
                if (count == 5)
                {
                    GetResults();
                }
            }
        }
    }

    public void ResetGenerator()
    {
        atts = new int[5];
        attLimits = new int[5];
        social = new int[5];
        for (int i = 0; i < social.Length; i++)
        {
            social[i] = 1;
        }
        skills = new int[12];
        typeResults = new int[18];
        resultsWindow.SetActive(false);
        resetBut.SetActive(false);
        resultsDex.SetActive(false);
    }
    void SetSkills()
    {
        if (rank >= 0)
            AddSkill(5);
        if (rank >= 1)
            AddSkill(4);
        if(rank >= 2)
            AddSkill(3);
        if (rank >= 3)
            AddSkill(2);
        if (rank >= 4)
            AddSkill(1);
    }
    void AddSkill(int num)
    {
        int r = num;
        while (r > 0)
        {
            int skillAdd = Random.Range(0, 12);
            if (skills[skillAdd] == 6 - num)
                continue;
            else
            {
                skills[skillAdd]++;
                r--;
            }
        }
    }
    void SetResults()
    {
        resultObjs[0].text = "Strength: " + atts[0];
        resultObjs[1].text = "Dexterity: " + atts[1];
        resultObjs[2].text = "Vitality: " + atts[2];
        resultObjs[3].text = "Special: " + atts[3];
        resultObjs[4].text = "Insight: " + atts[4];

        resultObjs[5].text = "Tough: " + social[0];
        resultObjs[6].text = "Cool: " + social[1];
        resultObjs[7].text = "Beauty: " + social[2];
        resultObjs[8].text = "Clever: " + social[3];
        resultObjs[9].text = "Cute: " + social[4];

        resultObjs[10].text = "HP: " + HP;
        resultObjs[11].text = "Will: " + will;
        resultObjs[12].text = "Nature: " + nature;

        resultObjs[13].text = "Brawl: " + skills[0];
        resultObjs[14].text = "Channel: " + skills[1];
        resultObjs[15].text = "Clash: " + skills[2];
        resultObjs[16].text = "Evasion: " + skills[3];
        resultObjs[17].text = "Alert: " + skills[4];
        resultObjs[18].text = "Athletic: " + skills[5];
        resultObjs[19].text = "Nature: " + skills[6];
        resultObjs[20].text = "Stealth: " + skills[7];
        resultObjs[21].text = "Allure: " + skills[8];
        resultObjs[22].text = "Etiquette: " + skills[9];
        resultObjs[23].text = "Intimidate: " + skills[10];
        resultObjs[24].text = "Perform: " + skills[11];
    }
    void SetRanks()
    {
        ranks[0] = "Starter";
        ranks[1] = "Beginner";
        ranks[2] = "Amateur";
        ranks[3] = "Ace";
        ranks[4] = "Pro";
    }
    void SetTypes()
    {
        types[0] = "Normal";
        types[1] = "Bug";
        types[2] = "Dark";
        types[3] = "Dragon";
        types[4] = "Electric";
        types[5] = "Fairy";
        types[6] = "Fight";
        types[7] = "Fire";
        types[8] = "Flying";
        types[9] = "Ghost";
        types[10] = "Grass";
        types[11] = "Ground";
        types[12] = "Ice";
        types[13] = "Poison";
        types[14] = "Psychic";
        types[15] = "Rock";
        types[16] = "Steel";
        types[17] = "Water";

        for(int i = 0; i < 18; i++)
        {
            for(int x = 0; x < 18; x++)
            {
                typesAdvs[i][x] = 0;
            }
        }
        typesAdvs[0][6] = -1;
        typesAdvs[0][9] = 5;
        typesAdvs[1][6] = 1;
        typesAdvs[1][10] = 1;
        typesAdvs[1][11] = 1;
        typesAdvs[1][7] = -1;
        typesAdvs[1][8] = -1;
        typesAdvs[1][15] = -1;
        typesAdvs[2][2] = 1;
        typesAdvs[2][9] = 1;
        typesAdvs[2][1] = -1;
        typesAdvs[2][5] = -1;
        typesAdvs[2][6] = -1;
        typesAdvs[2][14] = 5;
        typesAdvs[3][4] = 1;
        typesAdvs[3][7] = 1;
        typesAdvs[3][10] = 1;
        typesAdvs[3][17] = 1;
        typesAdvs[3][3] = -1;
        typesAdvs[3][5] = -1;
        typesAdvs[3][12] = -1;
        typesAdvs[4][4] = 1;
        typesAdvs[4][8] = 1;
        typesAdvs[4][16] = 1;
        typesAdvs[4][11] = -1;
        typesAdvs[5][1] = 1;
        typesAdvs[5][2] = 1;
        typesAdvs[5][6] = 1;
        typesAdvs[5][13] = -1;
        typesAdvs[5][16] = -1;
        typesAdvs[5][3] = 5;
        typesAdvs[6][1] = 1;
        typesAdvs[6][2] = 1;
        typesAdvs[6][15] = 1;
        typesAdvs[6][5] = -1;
        typesAdvs[6][8] = -1;
        typesAdvs[6][14] = -1;
        typesAdvs[7][1] = 1;
        typesAdvs[7][5] = 1;
        typesAdvs[7][7] = 1;
        typesAdvs[7][10] = 1;
        typesAdvs[7][12] = 1;
        typesAdvs[7][16] = 1;
        typesAdvs[7][11] = -1;
        typesAdvs[7][15] = -1;
        typesAdvs[7][17] = -1;
        typesAdvs[8][1] = 1;
        typesAdvs[8][6] = 1;
        typesAdvs[8][10] = 1;
        typesAdvs[8][4] = -1;
        typesAdvs[8][12] = -1;
        typesAdvs[8][15] = -1;
        typesAdvs[8][11] = 5;
        typesAdvs[9][1] = 1;
        typesAdvs[9][13] = 1;
        typesAdvs[9][2] = -1;
        typesAdvs[9][9] = -1;
        typesAdvs[9][6] = 5;
        typesAdvs[9][0] = 5;
        typesAdvs[10][4] = 1;
        typesAdvs[10][10] = 1;
        typesAdvs[10][11] = 1;
        typesAdvs[10][17] = 1;
        typesAdvs[10][1] = -1;
        typesAdvs[10][7] = -1;
        typesAdvs[10][8] = -1;
        typesAdvs[10][12] = -1;
        typesAdvs[10][13] = -1;
        typesAdvs[11][13] = 1;
        typesAdvs[11][15] = 1;
        typesAdvs[11][10] = -1;
        typesAdvs[11][12] = -1;
        typesAdvs[11][17] = -1;
        typesAdvs[11][4] = 5;
        typesAdvs[12][12] = 1;
        typesAdvs[12][6] = -1;
        typesAdvs[12][7] = -1;
        typesAdvs[12][15] = -1;
        typesAdvs[12][16] = -1;
        typesAdvs[13][1] = 1;
        typesAdvs[13][5] = 1;
        typesAdvs[13][6] = 1;
        typesAdvs[13][10] = 1;
        typesAdvs[13][13] = 1;
        typesAdvs[13][11] = -1;
        typesAdvs[13][14] = -1;
        typesAdvs[14][6] = 1;
        typesAdvs[14][14] = 1;
        typesAdvs[14][1] = -1;
        typesAdvs[14][2] = -1;
        typesAdvs[14][9] = -1;
        typesAdvs[15][7] = 1;
        typesAdvs[15][8] = 1;
        typesAdvs[15][0] = 1;
        typesAdvs[15][13] = 1;
        typesAdvs[15][10] = -1;
        typesAdvs[15][11] = -1;
        typesAdvs[15][6] = -1;
        typesAdvs[15][16] = -1;
        typesAdvs[15][17] = -1;
        typesAdvs[16][1] = 1;
        typesAdvs[16][3] = 1;
        typesAdvs[16][8] = 1;
        typesAdvs[16][5] = 1;
        typesAdvs[16][10] = 1;
        typesAdvs[16][12] = 1;
        typesAdvs[16][0] = 1;
        typesAdvs[16][14] = 1;
        typesAdvs[16][15] = 1;
        typesAdvs[16][16] = 1;
        typesAdvs[16][6] = -1;
        typesAdvs[16][7] = -1;
        typesAdvs[16][11] = -1;
        typesAdvs[16][13] = 5;
        typesAdvs[17][7] = 1;
        typesAdvs[17][12] = 1;
        typesAdvs[17][16] = 1;
        typesAdvs[17][17] = 1;
        typesAdvs[17][4] = -1;
        typesAdvs[17][10] = -1;
    }
    void SetNatures()
    {
        natures[0] = "Adamant 4";
        natures[1] = "Bashful 6";
        natures[2] = "Bold 9";
        natures[3] = "Brave 9";
        natures[4] = "Calm 8";
        natures[5] = "Careful 5";
        natures[6] = "Docile 7";
        natures[7] = "Gentle 10";
        natures[8] = "Hardy 9";
        natures[9] = "Hasty 7";
        natures[10] = "Impish 7";
        natures[11] = "Jolly 10";
        natures[12] = "Lax 8";
        natures[13] = "Lonely 5";
        natures[14] = "Mild 8";
        natures[15] = "Modest 10";
        natures[16] = "Naive 7";
        natures[17] = "Naughty 6";
        natures[18] = "Quiet 5";
        natures[19] = "Quirky 9";
        natures[20] = "Rash 6";
        natures[21] = "Relaxed 8";
        natures[22] = "Sassy 7";
        natures[23] = "Serious 4";
        natures[24] = "Timid 4";
    }
}
