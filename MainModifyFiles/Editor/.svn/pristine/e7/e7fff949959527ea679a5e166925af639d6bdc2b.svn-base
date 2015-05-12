using System;
using System.Collections.Generic;

namespace CodeGeneration
{
	public partial class UIViewCodeTemplate
	{
		public string className;
		public List<UIComponentInfo> source;
	
		public UIViewCodeTemplate (string className, List<UIComponentInfo> source)
		{
			if (string.IsNullOrEmpty (className)) {
				throw new System.ArgumentException ("className");
			}
	
			if (source == null) {
				throw new System.ArgumentNullException ("source cannot be null!");
			}
	
			this.className = className;
			this.source = source;
		}
	}


	public partial class UIViewCodeTemplateToLua
	{
		public string className;
		public List<UIComponentInfo> source;
		
		public UIViewCodeTemplateToLua (string className, List<UIComponentInfo> source)
		{
			if (string.IsNullOrEmpty (className)) {
				throw new System.ArgumentException ("className");
			}
			
			if (source == null) {
				throw new System.ArgumentNullException ("source cannot be null!");
			}
			
			this.className = className;
			this.source = source;
		}
	}
}