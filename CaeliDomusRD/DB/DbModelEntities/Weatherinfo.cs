using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace CaeliDomusRD.DbModelEntities {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class Weatherinfo {

		[JsonProperty, Column(IsPrimary = true)]
		public int ID { get; set; }

	}

}
