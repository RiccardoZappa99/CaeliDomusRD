using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace CaeliDomusRD.DbModelEntities {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class HomeSetting {

		[JsonProperty, Column(IsPrimary = true)]
		public int Id { get; set; }

		[JsonProperty]
		public float Humidity { get; set; }

		[JsonProperty]
		public float LightIntensity { get; set; }

		[JsonProperty]
		public float Pressure { get; set; }

		[JsonProperty, Column(DbType = "datetime")]
		public DateTime ReadTime { get; set; }

		[JsonProperty]
		public float Temperature { get; set; }

	}

}
