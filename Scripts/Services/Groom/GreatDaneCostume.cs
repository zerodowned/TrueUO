using Server.Mobiles;
using System;

namespace Server.Items
{
    public class GreatDaneCostume : BasePetCostume
    {
        public override int LabelNumber => 1159757; // Great Dane Costume

        [Constructable]
        public GreatDaneCostume()
            : base(0xA76D)
        {
        }        

        public GreatDaneCostume(Serial serial)
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
