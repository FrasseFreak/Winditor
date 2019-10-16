using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindEditor.ViewModel;
using OpenTK;

namespace WindEditor
{
	public partial class tag_light
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
            if (Unknown_1 == 1)
            {
                // TODO: model needs to be visually scaled twice as tall, without affecting the actual SCOB's scale
                //Transform.LocalScale = new Vector3(Transform.LocalScale.X, 2 * Transform.LocalScale.Y, Transform.LocalScale.Z);
                if (Unknown_4 >= 9)
                {
                    m_actorMeshes = WResourceManager.LoadActorResource("Light Ray Cylinder");
                }
                else
                {
                    m_actorMeshes = WResourceManager.LoadActorResource("Light Ray Cone");
                }
            }
            else
            {
                m_objRender = WResourceManager.LoadObjResource("resources/editor/EditorCube.obj", new OpenTK.Vector4(1f, 1f, 1f, 1f));
            }
        }
	}
}
