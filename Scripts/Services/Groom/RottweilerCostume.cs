using Server.Mobiles;
using System;

namespace Server.Items
{
    public class RottweilerCostume : BasePetCostume
    {
        public override int LabelNumber => 1159760; // Rottweiler Costume

        [Constructable]
        public RottweilerCostume()
            : base(0xA770)
        {
        }        

        public RottweilerCostume(Serial serial)
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
