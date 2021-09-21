using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Gumps
{
    public class GroomGump : Gump
    {
        public readonly PetGroomer Groomer;
        public BaseCreature Creature;
        public List<PetCostumeSubDefiniton> List;        

        public GroomGump(PetGroomer groomer, BaseCreature bc, List<PetCostumeSubDefiniton> list)
            : base(50, 50)
        {
            Groomer = groomer;
            Creature = bc;
            List = list;

            int grid = list.Count < 7 ? 3 : 4;
            bool wide = grid == 4;

            AddPage(0);            

            AddBackground(0, 0, wide ? 570 : 432, wide ? 360 : 274, 0x6DB);

            AddButton(wide ? 136 : 107, wide ? 312 : 233, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(wide ? 168 : 139, wide ? 312 : 233, 90, 35, 1006044, 0x7FFF, false, false); // OK

            AddButton(wide ? 377 : 296, wide ? 312 : 233, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(wide ? 409 : 328, wide ? 312 : 233, 90, 35, 1006045, 0x7FFF, false, false); // Cancel

            AddHtmlLocalized(11, 23, wide ? 550 : 412, 20, 1159772, bc.Name, 0x7FFF, false, false); // <DIV ALIGN=CENTER>Grooming your pet named ~1_NAME~</DIV>

            int x = 70;
            int y = 60;
            int i = 0;

            foreach (var l in list)
            {
                var itemID = ShrinkTable.Lookup(l.CostumeBodyID);

                if (i == grid)
                {
                    i = 0;
                    y += 83;
                }

                AddBackground(x + (i * 124), y, 70, 74, 0x9D60);
                AddTooltip(l.Cliloc);
                AddItem(x + 10 + (i * 124), y + 20, itemID, l.CostumeHue);
                AddRadio(x - 40 + (i * 124), y + 25, 0xD2, 0xD3, false, 9999 + i);

                i++;
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (!Creature.Alive)
            {
                return;
            }

            if (Creature.Map != from.Map || !Creature.InRange(Groomer, 12) || !from.InRange(Groomer, 12))
            {
                from.SendLocalizedMessage(1049538); // That creature is too far away.
            }

            if (Creature.Mounted)
            {
                from.SendLocalizedMessage(1042146); // You cannot use this while mounted.
                return;
            }

            if (Creature.IsBodyMod || Creature.HueMod > -1)
            {
                from.SendLocalizedMessage(1159776); // Pets that are which are under appearance-altering effects cannot be groomed.
                return;
            }

            if (info.ButtonID == 1)
            {
                int[] switches = info.Switches;

                if (switches.Length > 0)
                {
                    int index = switches[0];

                    var entry = List[index - 9999];

                    if (Creature.BodyValue == entry.CostumeBodyID)
                    {
                        from.SendLocalizedMessage(1159774); // Your pet is already using this grooming option.
                        return;
                    }

                    Creature.BodyValue = entry.CostumeBodyID;

                    if (Creature is BaseMount bm)
                    {
                        bm.ItemID = entry.CostumeItemID;
                    }

                    Creature.Hue = entry.CostumeHue;

                    from.SendLocalizedMessage(1159765); // You have successfully groomed your pet.
                }
            }
            else
            {
                from.SendLocalizedMessage(1159766); // You decide not to groom your pet.
            }
        }
    }
}
