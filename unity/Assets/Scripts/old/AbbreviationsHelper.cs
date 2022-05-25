using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbbreviationsHelper : MonoBehaviour
{
    private Dictionary<string, string> abbreviations = new Dictionary<string, string>();
    private Dictionary<string, string> abbreviations_rev = new Dictionary<string, string>();
    private Dictionary<string, string> abbreviations_items = new Dictionary<string, string>();
    private Dictionary<string, string[]> profession_items = new Dictionary<string, string[]>();
    public void Start()
    {
        abbreviations.Add("BWOOD", "Birch Wood");
        abbreviations.Add("WOOL", "Wool");
        abbreviations.Add("TBAR", "Tin Bar");
        abbreviations.Add("OWOOD", "Oak Wood");
        abbreviations.Add("LINEN", "Linen");
        abbreviations.Add("IBAR", "Iron Bar");
        abbreviations.Add("TWOOD", "Teak Wood");
        abbreviations.Add("SILK", "Silk");
        abbreviations.Add("CO", "Copper Ore");
        abbreviations.Add("BIRCH", "Birch");
        abbreviations.Add("COTTON", "Cotton");
        abbreviations.Add("TO", "Tin Ore");
        abbreviations.Add("OAK", "Oak");
        abbreviations.Add("FLAX", "Flax");
        abbreviations.Add("IO", "Iron Ore");
        abbreviations.Add("TEAK", "Teak");
        abbreviations.Add("SWORMS", "Silk Worms");
        abbreviations.Add("CB", "Copper Bar");

        abbreviations_rev.Add("Birch Wood", "BWOOD");
        abbreviations_rev.Add("Wool", "WOOL");
        abbreviations_rev.Add("Tin Bar", "TBAR");
        abbreviations_rev.Add("Oak Wood", "OWOOD");
        abbreviations_rev.Add("Linen", "LINEN");
        abbreviations_rev.Add("Iron Bar", "IBAR");
        abbreviations_rev.Add("Teak Wood", "TWOOD");
        abbreviations_rev.Add("Silk", "SILK");
        abbreviations_rev.Add("Copper Ore", "CO");
        abbreviations_rev.Add("Birch", "BIRCH");
        abbreviations_rev.Add("Cotton", "COTTON");
        abbreviations_rev.Add("Tin Ore", "TO");
        abbreviations_rev.Add("Oak", "OAK");
        abbreviations_rev.Add("Flax", "FLAX");
        abbreviations_rev.Add("Iron Ore", "IO");
        abbreviations_rev.Add("Teak", "TEAK");
        abbreviations_rev.Add("Silk Worms", "SWORMS");
        abbreviations_rev.Add("Copper Bar", "CB");

        string[] miner_items = { 
            "Copper Hammer and Chisel", 
            "Copper Pickaxe",
            "Birch Mining Cart",
            "Tin Hammer and Chisel",
            "Tin Pickaxe",
            "Oak Mining Cart",
            "Iron Pickaxe",
            "Teak Mining Cart",
            "Iron Hammer and Chisel",
        };
        string[] lumber_items = {
            "Copper Saw",
            "Tin Saw",
            "Iron Saw",
            "Copper Axe",
            "Tin Axe",
            "Iron Axe",
            "Birch Wheelbarrow",
            "Oak Wheelbarrow",
            "Teak Wheelbarrow",
        };
        string[] farmer_items = {
            "Copper Sickle",
            "Tin Sickle",
            "Iron Sickle",
            "Copper Hoe",
            "Tin Hoe",
            "Iron Hoe",
            "Birch Wagon",
            "Oak Wagon",
            "Teak Wagon",
        };
        profession_items.Add("Miner",miner_items);
        profession_items.Add("Lumberjack",lumber_items);
        profession_items.Add("Farmer",farmer_items);
    }

    public void add_items()
    {
        abbreviations_items.Add("Copper Hammer and Chisel", "Miner");
        abbreviations_items.Add("Copper Pickaxe", "Miner");
        abbreviations_items.Add("Birch Mining Cart", "Miner");
        abbreviations_items.Add("Tin Hammer and Chisel", "Miner");
        abbreviations_items.Add("Tin Pickaxe", "Miner");
        abbreviations_items.Add("Oak Mining Cart", "Miner");
        abbreviations_items.Add("Iron Pickaxe", "Miner");
        abbreviations_items.Add("Teak Mining Cart", "Miner");
        abbreviations_items.Add("Iron Hammer and Chisel", "Miner");
        abbreviations_items.Add("Copper Saw", "Lumberjack");
        abbreviations_items.Add("Tin Saw", "Lumberjack");
        abbreviations_items.Add("Iron Saw", "Lumberjack");
        abbreviations_items.Add("Copper Axe", "Lumberjack");
        abbreviations_items.Add("Tin Axe", "Lumberjack");
        abbreviations_items.Add("Iron Axe", "Lumberjack");
        abbreviations_items.Add("Birch Wheelbarrow", "Lumberjack");
        abbreviations_items.Add("Oak Wheelbarrow", "Lumberjack");
        abbreviations_items.Add("Teak Wheelbarrow", "Lumberjack");
        abbreviations_items.Add("Copper Sickle", "Farmer");
        abbreviations_items.Add("Tin Sickle", "Farmer");
        abbreviations_items.Add("Iron Sickle", "Farmer");
        abbreviations_items.Add("Copper Hoe", "Farmer");
        abbreviations_items.Add("Tin Hoe", "Farmer");
        abbreviations_items.Add("Iron Hoe", "Farmer");
        abbreviations_items.Add("Birch Wagon", "Farmer");
        abbreviations_items.Add("Oak Wagon", "Farmer");
        abbreviations_items.Add("Teak Wagon", "Farmer");
    }

    public Dictionary<string,string> mat_abv
    {
        get { return abbreviations; }
    }
    
    public Dictionary<string,string> mat_abv_rev
    {
        get { return abbreviations_rev; }
    }

    public Dictionary<string,string> equip_items_abv
    {
        get { return abbreviations_items; }
    }

    public Dictionary<string,string[]> profession_equip_items
    {
        get { return profession_items; }
    }

}
