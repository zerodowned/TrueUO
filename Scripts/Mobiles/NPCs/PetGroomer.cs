using Server.ContextMenus;
using Server.Gumps;
using Server.Targeting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Mobiles
{
    public class PlayerCostumeList
    {
        public PlayerMobile Owner { get; set; }
        public List<int> Costumes { get; set; }

        public PlayerCostumeList(PlayerMobile owner, int cliloc)
        {
            Owner = owner;

            if (Costumes == null)
            {
                Costumes = new List<int>();
            }

            Costumes.Add(cliloc);
        }
    }

    public class PetCostumeList
    {
        public BaseMount Pet { get; set; }
        public int OrginalBodyID { get; set; }
        public int OrginalItemID { get; set; }
        public int OrginalHue { get; set; }        
        public List<int> Costumes { get; set; }

        public PetCostumeList(BaseMount pet, int cliloc)
            : this(pet, cliloc, pet.BodyValue, pet.ItemID, pet.Hue)
        {
        }

        public PetCostumeList(BaseMount pet, int cliloc, int bv, int iid, int hue)
        {
            Pet = pet;
            OrginalBodyID = bv;
            OrginalItemID = iid;
            OrginalHue = hue;

            if (Costumes == null)
            {
                Costumes = new List<int>();
            }

            Costumes.Add(cliloc);
        }
    }

    public class PetCostumeDefiniton
    {
        public Type CreatureType { get; }
        public PetCostumeSubDefiniton Type { get; }
        public PetCostumeSubDefiniton[] Types { get; }

        public PetCostumeDefiniton(Type ctype, PetCostumeSubDefiniton type, PetCostumeSubDefiniton[] types)
        {
            CreatureType = ctype;
            Type = type;
            Types = types;
        }
    }

    public class PetCostumeSubDefiniton
    {
        public int Cliloc { get; set; }     
        public int CostumeItemID { get; set; }
        public int CostumeBodyID { get; set; }
        public int CostumeHue { get; set; }

        public PetCostumeSubDefiniton(int cliloc, int cid, int cbid)
        {
            Cliloc = cliloc;
            CostumeItemID = cid;
            CostumeBodyID = cbid;
        }

        public PetCostumeSubDefiniton(int cliloc, int cid, int cbid, int chue)
        {
            Cliloc = cliloc;
            CostumeItemID = cid;
            CostumeBodyID = cbid;
            CostumeHue = chue;
        }
    }

    public class PetGroomer : AnimalTrainer
    {
        public static string FilePath = Path.Combine("Saves/Misc", "Groomer.bin");
        public static List<PlayerCostumeList> PlayerList = new List<PlayerCostumeList>();
        public static List<PetCostumeList> PetList = new List<PetCostumeList>();

        public static void Configure()
        {
            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(
                FilePath,
                writer =>
                {
                    writer.Write(0);

                    writer.Write(PlayerList.Count);

                    for (var i = 0; i < PlayerList.Count; i++)
                    {
                        var s = PlayerList[i];

                        writer.Write(s.Owner);
                        writer.Write(s.Costumes.Count);

                        for (var j = 0; j < s.Costumes.Count; j++)
                        {
                            var c = s.Costumes[j];

                            writer.Write(c);
                        }
                    }

                    writer.Write(PetList.Count);

                    for (var k = 0; k < PetList.Count; k++)
                    {
                        var s = PetList[k];

                        writer.Write(s.Pet);
                        writer.Write(s.OrginalBodyID);
                        writer.Write(s.OrginalItemID);
                        writer.Write(s.OrginalHue);

                        writer.Write(s.Costumes.Count);

                        for (var l = 0; l < s.Costumes.Count; l++)
                        {
                            var c = s.Costumes[l];

                            writer.Write(c);
                        }
                    }
                });
        }

        public static void OnLoad()
        {
            Persistence.Deserialize(
                FilePath,
                reader =>
                {
                    int version = reader.ReadInt();
                    int playercount = reader.ReadInt();

                    for (int i = playercount; i > 0; i--)
                    {
                        PlayerMobile pm = (PlayerMobile)reader.ReadMobile();

                        int costumecount = reader.ReadInt();

                        for (int j = costumecount; j > 0; j--)
                        {
                            int value = reader.ReadInt();

                            if (pm != null)
                            {
                                AddPlayerCostume(pm, value);
                            }
                        }
                    }

                    int petcount = reader.ReadInt();

                    for (int k = petcount; k > 0; k--)
                    {
                        BaseMount mount = (BaseMount)reader.ReadMobile();
                        int bv = reader.ReadInt();
                        int iid = reader.ReadInt();
                        int hue = reader.ReadInt();

                        int costumecount = reader.ReadInt();

                        for (int l = costumecount; l > 0; l--)
                        {
                            int cliloc = reader.ReadInt();

                            if (mount != null)
                            {
                                AddPetCostume(mount, cliloc, bv, iid, hue);
                            }
                        }
                    }
                });
        }

        public static PetCostumeDefiniton[] _Table =
        {
            new PetCostumeDefiniton(typeof(CuSidhe), new PetCostumeSubDefiniton(1159754, 16017, 277), new[] // Cu Sidhe Costume
            {
                new PetCostumeSubDefiniton(1159755, 16084, 1546, 0x0), // Newfoundland Costume
                new PetCostumeSubDefiniton(1159756, 16085, 1547, 0x0), // Malamute Costume
                new PetCostumeSubDefiniton(1159757, 16086, 1548, 0x0), // Great Dane Costume
                new PetCostumeSubDefiniton(1159758, 16087, 1549, 0x0), // Saint Bernard Costume
                new PetCostumeSubDefiniton(1159759, 16088, 1551, 0x0), // Russian Terrier Costume
                new PetCostumeSubDefiniton(1159760, 16089, 1552, 0x0) // Rottweiler Costume
            })
        };

        public static void AddPlayerCostume(PlayerMobile owner, int cliloc)
        {
            var own = PlayerList.FirstOrDefault(x => x.Owner == owner);

            if (own == null)
            {
                PlayerList.Add(new PlayerCostumeList(owner, cliloc));
            }
            else
            {
                own.Costumes.Add(cliloc);
            }
        }

        public static void PlayerListRemove(PlayerMobile pm, int cliloc)
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].Owner == pm)
                {
                    for (int j = 0; j < PlayerList[i].Costumes.Count; j++)
                    {
                        int value = PlayerList[i].Costumes[j];

                        if (value == cliloc)
                        {
                            PlayerList[i].Costumes.Remove(value);
                        }
                    }
                }
            }
        }

        public static void AddPetCostume(BaseMount mount, int cliloc)
        {
            var bm = PetList.FirstOrDefault(x => x.Pet == mount);

            if (bm == null)
            {
                PetList.Add(new PetCostumeList(mount, cliloc));
            }
            else
            {
                bm.Costumes.Add(cliloc);
            }
        }

        public static void AddPetCostume(BaseMount mount, int cliloc, int bv, int iid, int hue)
        {
            var bm = PetList.FirstOrDefault(x => x.Pet == mount);

            if (bm == null)
            {
                PetList.Add(new PetCostumeList(mount, cliloc, bv, iid, hue));
            }
            else
            {
                bm.Costumes.Add(cliloc);
            }
        }

        public static void PetListRemoveAll(BaseCreature bc)
        {
            for (int i = 0; i < PetList.Count; i++)
            {
                var value = PetList[i];

                if (value.Pet == bc)
                {
                    PetList.Remove(value);
                }
            }
        }

        public static bool IsInCostume(BaseCreature bc)
        {
            if (bc is BaseMount bm)
            {
                foreach (var costume in _Table)
                {
                    if (costume.CreatureType == bc.GetType())
                    {
                        foreach (var t in costume.Types)
                        {
                            if (t.CostumeBodyID == bm.BodyValue)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static void ReturnCostume(BaseCreature bc)
        {
            var petlist = PetList.FirstOrDefault(x => x.Pet == bc);

            if (bc.ControlMaster != null)
            {
                var playercostumes = PlayerList.FirstOrDefault(x => x.Owner == bc.ControlMaster).Costumes;

                foreach (var costume in petlist.Costumes)
                {
                    playercostumes.Add(costume);
                }

                bc.BodyValue = petlist.OrginalBodyID;

                if (bc is BaseMount bm)
                {
                    bm.ItemID = petlist.OrginalItemID;
                }

                bc.Hue = petlist.OrginalHue;

                PetListRemoveAll(bc);

                bc.ControlMaster.SendLocalizedMessage(1159790); // All costume options found on this pet have been returned to you.
            }
        }        

        public static PetCostumeSubDefiniton GetProp(int cliloc)
        {
            PetCostumeSubDefiniton p = null;

            foreach (var costume in _Table)
            {
                if (costume.Type.Cliloc == cliloc)
                {
                    p = costume.Type;
                }

                foreach (var t in costume.Types)
                {
                    if (t.Cliloc == cliloc)
                    {
                        p = t;
                    }
                }
            }

            return p;
        }

        public static void SetBody(BaseMount bm, int cliloc)
        {
            foreach (var costume in _Table)
            {
                if (costume.CreatureType == bm.GetType())
                {
                    foreach (var t in costume.Types)
                    {
                        if (t.Cliloc == cliloc)
                        {
                            bm.BodyValue = t.CostumeBodyID;
                            bm.ItemID = t.CostumeItemID;
                            bm.Hue = t.CostumeHue;                            
                        }
                    }
                }
            }
        }        

        public static bool CreatureTypeCheck(BaseCreature bc, int cliloc)
        {
            foreach (var costume in _Table)
            {
                if (costume.CreatureType == bc.GetType())
                {
                    foreach (var t in costume.Types)
                    {
                        if (t.Cliloc == cliloc)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        [Constructable]
        public PetGroomer()
            : base("the pet groomer")
        {
        }

        public PetGroomer(Serial serial)
            : base(serial)
        { }


        public override void InitSBInfo()
        {
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive)
            {
                list.Add(new GroomPetEntry(this, from));
                list.Add(new ApplyStoreCostumeEntry(this, from));
            }

            base.AddCustomContextEntries(from, list);
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

        private class GroomPetEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly PetGroomer m_Trainer;

            public GroomPetEntry(PetGroomer trainer, Mobile from)
                : base(1159762, 12) // Groom Pet
            {
                m_Trainer = trainer;
                m_From = from;
            }

            public override void OnClick()
            {
                m_Trainer.BeginGroom(m_From);
            }
        }

        public void BeginGroom(Mobile from)
        {
            if (Deleted || !from.CheckAlive())
            {
                return;
            }

            from.SendLocalizedMessage(1159769); // Target the pet you wish to groom.

            from.Target = new GroomTarget(this);
        }

        private class GroomTarget : Target
        {
            private readonly PetGroomer Groomer;

            public GroomTarget(PetGroomer groomer)
                : base(12, false, TargetFlags.None)
            {
                Groomer = groomer;
            }

            public List<PetCostumeSubDefiniton> Check(BaseCreature bc)
            {
                List<PetCostumeSubDefiniton> list = null;

                var l = PetList.FirstOrDefault(x => x.Pet == bc);

                if (l != null)
                {
                    foreach (var t in _Table)
                    {
                        if (t.CreatureType == bc.GetType())
                        {
                            if (list == null)
                            {
                                list = new List<PetCostumeSubDefiniton>();
                            }

                            list.Add(new PetCostumeSubDefiniton(t.Type.Cliloc, l.OrginalItemID, l.OrginalBodyID, l.OrginalHue));

                            foreach (var costume in t.Types)
                            {
                                if (l.Costumes.Contains(costume.Cliloc))
                                {
                                    list.Add(costume);
                                }
                            }
                        }
                    }
                }

                return list;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature bc)
                {
                    if (!bc.Alive)
                    {
                        return;
                    }

                    if (bc.ControlMaster == from && bc.Controlled)
                    {
                        if (bc.IsBonded)
                        {
                            var g = Check(bc);

                            if (g != null)
                            {
                                from.CloseGump(typeof(GroomGump));
                                from.SendGump(new GroomGump(Groomer, bc, g));
                            }
                            else
                            {
                                from.SendLocalizedMessage(1159778); // No pet costumes have been applied to this pet. Visit the Ultima Store for more details about pet costumes.
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1159775); // Only a bonded pet can be groomed.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1149667); // Invalid target.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1149667); // Invalid target.
                }
            }
        }

        private class ApplyStoreCostumeEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly PetGroomer m_Trainer;

            public ApplyStoreCostumeEntry(PetGroomer trainer, Mobile from)
                : base(1159771, 12) // Apply Store Costume
            {
                m_Trainer = trainer;
                m_From = from;
            }

            public override void OnClick()
            {
                m_Trainer.BeginApplyStoreCostume(m_From);
            }
        }

        public void BeginApplyStoreCostume(Mobile from)
        {
            if (Deleted || !from.CheckAlive())
            {
                return;
            }

            from.SendLocalizedMessage(1159769); // Target the pet you wish to groom.

            from.Target = new ApplyStoreCostumeTarget(this);
        }

        private class ApplyStoreCostumeTarget : Target
        {
            private readonly PetGroomer Groomer;

            public ApplyStoreCostumeTarget(PetGroomer groomer)
                : base(12, false, TargetFlags.None)
            {
                Groomer = groomer;
            }

            public List<PetCostumeSubDefiniton> Check(Mobile from, BaseCreature bc)
            {
                List<PetCostumeSubDefiniton> list = null;

                foreach (var l in PlayerList)
                {
                    if (l.Owner == from && l.Costumes != null)
                    {
                        foreach (var p in l.Costumes)
                        {
                            if (CreatureTypeCheck(bc, p))
                            {
                                if (list == null)
                                {
                                    list = new List<PetCostumeSubDefiniton>();
                                }

                                var prop = GetProp(p);

                                list.Add(prop);
                            }
                        }
                    }
                }

                return list;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseMount bc)
                {
                    if (!bc.Alive)
                    {
                        return;
                    }

                    if (bc.ControlMaster == from && bc.Controlled)
                    {
                        if (bc.IsBonded)
                        {
                            var g = Check(from, bc);

                            if (g != null)
                            {
                                from.CloseGump(typeof(ApplyStoreCostumeGump));
                                from.SendGump(new ApplyStoreCostumeGump(Groomer, bc, g));
                            }
                            else
                            {
                                from.SendLocalizedMessage(1159777); // There are no available costumes credits for this pet type. Visit the Ultima Store for more details about pet costumes.
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1159775); // Only a bonded pet can be groomed.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1149667); // Invalid target.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1149667); // Invalid target.
                }
            }
        }
    }
}
