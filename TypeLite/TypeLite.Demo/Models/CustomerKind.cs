﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TypeLite.Demo.Models {
	[TsEnum(Module = "Eshop")]
	public enum CustomerKind {
		Corporate = 1,
		Individual = 2
	}
}