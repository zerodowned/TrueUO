using Server.Mobiles;

namespace Server.Items
{
    public partial class BasePetCostume : Item
    {
        public BasePetCostume(int id)
            : base(id)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 1019045);
            }
            else
            {
                foreach (var costume in PetGroomer._Table)
                {
                    foreach (var t in costume.Types)
                    {
                        if (t.Cliloc == LabelNumber)
                        {
                            PetGroomer.AddPlayerCostume((PlayerMobile)from, t.Cliloc);
                            from.SendLocalizedMessage(1159763, string.Format("#{0}", t.Cliloc)); // Your purchased ~1_COSTUME~ has been credited to your character. Visit the nearest pet groomer to apply this costume to your pet.
                        }
                    }
                }                
            }
        }

        public BasePetCostume(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
