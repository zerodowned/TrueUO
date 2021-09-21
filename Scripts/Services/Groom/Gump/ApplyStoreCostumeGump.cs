using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using System.Linq;

namespace Server.Gumps
{
    public class ApplyStoreCostumeGump : Gump
    {
        public readonly PetGroomer Groomer;
        public List<PetCostumeSubDefiniton> List;
        public BaseMount Creature;

        public ApplyStoreCostumeGump(PetGroomer groomer, BaseMount bc, List<PetCostumeSubDefiniton> list)
            : base(50, 50)
        {
            Groomer = groomer;
            Creature = bc;
            List = list;

            AddPage(0);

            AddBackground(0, 0, 432, 275, 0x6DB);

            AddButton(86, 234, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(118, 234, 90, 35, 1006044, 0x7FFF, false, false); // OK

            AddButton(238, 234, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(270, 234, 90, 35, 1006045, 0x7FFF, false, false); // Cancel

            AddHtmlLocalized(11, 23, 412, 20, 1159773, bc.Name, 0x7FFF, false, false); // <DIV ALIGN=CENTER>Applying a new costume to your pet named ~1_NAME~</DIV>

            int x = 70;
            int y = 60;
            int i = 0;

            foreach(var l in list)
            {
                var itemID = ShrinkTable.Lookup(l.CostumeBodyID);

                if (i == 3)
                {
                    i = 0;
                    y += 83;
                }

                AddBackground(x + (i * 124), y, 70, 74, 0x9D60);
                AddTooltip(l.Cliloc);
                AddItem(x + 10 + (i * 124), y + 20, itemID, l.CostumeHue);
                AddRadio(x - 40 + (i * 124), y + 25, 0xD2, 0xD3, false, 10000 + i);

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
            }

            if (info.ButtonID == 1)
            {
                int[] switches = info.Switches;

                if (switches.Length > 0)
                {
                    int index = switches[0];

                    var entry = List[index - 10000];

                    var pet = PetGroomer.PetList.FirstOrDefault(x => x.Pet == Creature);

                    if (pet != null && pet.Costumes.Contains(entry.Cliloc))
                    {
                        from.SendLocalizedMessage(1159774); // You have selected a costume that is either invalid or has already been applied to this pet.
                        return;
                    }

                    PetGroomer.AddPetCostume(Creature, entry.Cliloc);
                    PetGroomer.SetBody(Creature, entry.Cliloc);
                    PetGroomer.PlayerListRemove((PlayerMobile)from, entry.Cliloc);

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
