using Server.Mobiles;
using System;

namespace Server.Items
{
    public class NewfoundlandCostume : BasePetCostume
    {
        public override int LabelNumber => 1159755; // Newfoundland Costume

        [Constructable]
        public NewfoundlandCostume()
            : base(0xA76B)
        {
        }        

        public NewfoundlandCostume(Serial serial)
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
