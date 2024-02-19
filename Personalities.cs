using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.EventFilter
{
    internal static class Personalities
    {
        /* For future reference, from maid_status_personal_list.nei
            10,Pure
            20,Cool
            30,Pride
            40,Yandere
            50,Anesan
            60,Genki
            70,Sadist
            80,Muku
            90,Majime
            100,Rindere
            110,Silent
            120,Devilish
            130,Ladylike
            140,Secretary
            150,Sister
            160,Curtness無
            170,Missy嬢
            180,Childhood
            190,Masochist
            200,Crafty
            210,Friendly
            220,Dame
            230,Gal
        */

        internal static SortedDictionary<int, string> PersonalityDic => new()
        {
            {0, "..."},
            {10, "Pure"},
            {20, "Kuudere"},
            {30, "Tsundere"},            
            {40, "Yandere"},
            {50, "Onee-chan"},
            {60, "Genki"},
            {70, "Sadistic Queen"},
            {80, "Muku" },
            {90 , "Majime"},
            {100 , "Rindere"},
            {110 , "Bookworm"},
            {120 , "Koakuma"},
            {130 , "Ladylike"},
            {140 , "Secretary"},
            {150 , "Imouto"},
            {160 , "Wary"},
            {170 , "Ojousama"},
            {180 , "Osananajimi"},
            {190 , "Masochist"},
            {200 , "Haraguro"},
            {210 , "Kisakude"},
            {220 , "Kimajime"},
            {230,  "Gyaru"},
            {999 , "NPC & Special Only"},
            {1000, "Custom Filter Only" }
        };

        internal static string[] GetPersonalityArray()
        {
            return PersonalityDic.Values.ToArray();
        }

        internal static int GetId(int num)
        {
            int[] array = PersonalityDic.Keys.ToArray();
            return array[num];
        }
    }
}
