using Server.Mobiles;
using System;

namespace Server.Items
{
    public class SaintBernardCostume : BasePetCostume
    {
        public override int LabelNumber => 1159758; // Saint Bernard Costume

        [Constructable]
        public SaintBernardCostume()
            : base(0xA76E)
        {
        }        

        public SaintBernardCostume(Serial serial)
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
