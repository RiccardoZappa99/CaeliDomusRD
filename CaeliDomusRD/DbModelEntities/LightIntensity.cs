using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace CaeliDomusRD.DbModelEntities {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class LightIntensity {

		[JsonProperty, Column(IsPrimary = true)]
		public int ID { get; set; }

		[JsonProperty]
		public float LightValue { get; set; }

		[JsonProperty, Column(DbType = "datetime")]
		public DateTime ReadTime { get; set; }

	}

}
