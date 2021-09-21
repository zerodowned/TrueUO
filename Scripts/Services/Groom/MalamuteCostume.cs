using Server.Mobiles;
using System;

namespace Server.Items
{
    public class MalamuteCostume : BasePetCostume
    {
        public override int LabelNumber => 1159756; // Malamute Costume

        [Constructable]
        public MalamuteCostume()
            : base(0xA76C)
        {
        }        

        public MalamuteCostume(Serial serial)
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
