﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindEditor
{
    public partial class item
    {
        public override void PostLoad()
        {
            UpdateModel();
            base.PostLoad();
        }

        public override void PreSave()
        {

        }

        private void UpdateModel()
        {
            m_actorMeshes.Clear();
            m_objRender = null;
            switch (ItemID)
            {
                case ItemID.Heart:
                    m_actorMeshes = WResourceManager.LoadActorResource("Heart Pickup");
                    break;
                case ItemID.Green_Rupee:
                    m_actorMeshes = WResourceManager.LoadActorResource("Green Rupee");
                    break;
                case ItemID.Blue_Rupee:
                    m_actorMeshes = WResourceManager.LoadActorResource("Blue Rupee");
                    break;
                case ItemID.Yellow_Rupee:
                case ItemID.Yellow_Rupee_Joke_Message:
                    m_actorMeshes = WResourceManager.LoadActorResource("Yellow Rupee");
                    break;
                case ItemID.Red_Rupee:
                    m_actorMeshes = WResourceManager.LoadActorResource("Red Rupee");
                    break;
                case ItemID.Purple_Rupee:
                    m_actorMeshes = WResourceManager.LoadActorResource("Purple Rupee");
                    break;
                case ItemID.Orange_Rupee:
                    m_actorMeshes = WResourceManager.LoadActorResource("Orange Rupee");
                    break;
                case ItemID.Silver_Rupee:
                    m_actorMeshes = WResourceManager.LoadActorResource("Silver Rupee");
                    break;
                case ItemID.Small_Key:
                    m_actorMeshes = WResourceManager.LoadActorResource("Small Key");
                    break;
                case ItemID.Small_Magic_Jar:
                    m_actorMeshes = WResourceManager.LoadActorResource("Small Magic Jar");
                    break;
                case ItemID.Large_Magic_Jar:
                    m_actorMeshes = WResourceManager.LoadActorResource("Large Magic Jar");
                    break;
                case ItemID.Bombs_5:
                    m_actorMeshes = WResourceManager.LoadActorResource("Bombs Pickup");
                    break;
                case ItemID.Bombs_10:
                    m_actorMeshes = WResourceManager.LoadActorResource("Bombs Pickup");
                    break;
                case ItemID.Bombs_20:
                    m_actorMeshes = WResourceManager.LoadActorResource("Bombs Pickup");
                    break;
                case ItemID.Bombs_30:
                    m_actorMeshes = WResourceManager.LoadActorResource("Bombs Pickup");
                    break;
                case ItemID.Arrows_10:
                    m_actorMeshes = WResourceManager.LoadActorResource("Arrows Pickup");
                    break;
                case ItemID.Arrows_20:
                    m_actorMeshes = WResourceManager.LoadActorResource("Arrows Pickup");
                    break;
                case ItemID.Arrows_30:
                    m_actorMeshes = WResourceManager.LoadActorResource("Arrows Pickup");
                    break;
                case ItemID.Knights_Crest:
                    m_actorMeshes = WResourceManager.LoadActorResource("Knights Crest");
                    break;
                case ItemID.Joy_Pendant:
                    m_actorMeshes = WResourceManager.LoadActorResource("Joy Pendant");
                    break;
                case ItemID.Golden_Feather:
                    m_actorMeshes = WResourceManager.LoadActorResource("Golden Feather");
                    break;
                case ItemID.Boko_Baba_Seed:
                    m_actorMeshes = WResourceManager.LoadActorResource("Boko Baba Seed");
                    break;
                case ItemID.Skull_Necklace:
                    m_actorMeshes = WResourceManager.LoadActorResource("Skull Necklace");
                    break;
                case ItemID.Green_Chu_Jelly:
                    m_actorMeshes = WResourceManager.LoadActorResource("Green Chu Blob");
                    break;
                case ItemID.Blue_Chu_Jelly:
                    m_actorMeshes = WResourceManager.LoadActorResource("Blue Chu Blob");
                    break;
                case ItemID.Red_Chu_Jelly:
                    m_actorMeshes = WResourceManager.LoadActorResource("Red Chu Blob");
                    break;
                case ItemID.Bombs:
                    m_actorMeshes = WResourceManager.LoadActorResource("Bombs Ability");
                    break;
                default:
                    m_objRender = WResourceManager.LoadObjResource("resources/editor/EditorCube.obj", new OpenTK.Vector4(1f, 1f, 1f, 1f));
                    break;
            }
        }
    }
}
