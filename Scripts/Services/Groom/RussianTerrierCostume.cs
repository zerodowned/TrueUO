using Server.Mobiles;
using System;

namespace Server.Items
{
    public class RussianTerrierCostume : BasePetCostume
    {
        public override int LabelNumber => 1159759; // Russian Terrier Costume

        [Constructable]
        public RussianTerrierCostume()
            : base(0xA76F)
        {
        }        

        public RussianTerrierCostume(Serial serial)
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
