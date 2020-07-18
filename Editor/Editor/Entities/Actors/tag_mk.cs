using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindEditor.ViewModel;

namespace WindEditor
{
	public partial class tag_mk
	{
		public override void PostLoad()
		{
			base.PostLoad();

			m_RegionAreaModel = WResourceManager.LoadObjResource("resources/editor/EditorCube.obj", new OpenTK.Vector4(1f, 1f, 1f, 1f), true, false);
		}

		public override void PreSave()
		{

		}
	}
}
