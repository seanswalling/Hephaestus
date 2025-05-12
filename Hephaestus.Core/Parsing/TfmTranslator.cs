using System;
using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class TfmTranslator : ITfmTranslator
    {
        public Framework Translate(string? moniker)
        {
            if (string.IsNullOrWhiteSpace(moniker))
                throw new ArgumentNullException(nameof(moniker));

            moniker = moniker.Split(";")[0];

            if (MonikerDictionary.TryGetValue(moniker, out var value))
            {
                return value;
            }

            throw new ArgumentOutOfRangeException(nameof(moniker));
        }

        public string Translate(Framework framework)
        {
            return MonikerDictionary
                .Where(x => x.Key != "v4.8")
                .Single(x => x.Value == framework).Key;
        }

        private static readonly Dictionary<string, Framework> MonikerDictionary = new()
        {
            {"netcoreapp1.0"    ,   Framework.netcoreapp10  },
            {"netcoreapp1.1"    ,   Framework.netcoreapp11  },
            {"netcoreapp2.0"    ,   Framework.netcoreapp20  },
            {"netcoreapp2.1"    ,   Framework.netcoreapp21  },
            {"netcoreapp2.2"    ,   Framework.netcoreapp22  },
            {"netcoreapp3.0"    ,   Framework.netcoreapp30  },
            {"netcoreapp3.1"    ,   Framework.netcoreapp31  },
            {"net5.0"           ,   Framework.net50         },
            {"net6.0"           ,   Framework.net60         },
            {"net7.0"           ,   Framework.net70         },
            {"net8.0"           ,   Framework.net80         },
            {"net8"             ,   Framework.net80         },
            {"net9.0"           ,   Framework.net90         },
            {"netstandard1.0"   ,   Framework.netstandard10 },
            {"netstandard1.1"   ,   Framework.netstandard11 },
            {"netstandard1.2"   ,   Framework.netstandard12 },
            {"netstandard1.3"   ,   Framework.netstandard13 },
            {"netstandard1.4"   ,   Framework.netstandard14 },
            {"netstandard1.5"   ,   Framework.netstandard15 },
            {"netstandard1.6"   ,   Framework.netstandard16 },
            {"netstandard2.0"   ,   Framework.netstandard20 },
            {"netstandard2.1"   ,   Framework.netstandard21 },
            {"net11"            ,   Framework.net11         },
            {"net20"            ,   Framework.net20         },
            {"net35"            ,   Framework.net35         },
            {"net40"            ,   Framework.net40         },
            {"net403"           ,   Framework.net403        },
            {"net45"            ,   Framework.net45         },
            {"net451"           ,   Framework.net451        },
            {"net452"           ,   Framework.net452        },
            {"net46"            ,   Framework.net46         },
            {"net461"           ,   Framework.net461        },
            {"net462"           ,   Framework.net462        },
            {"net47"            ,   Framework.net47         },
            {"net471"           ,   Framework.net471        },
            {"net472"           ,   Framework.net472        },
            {"net48"            ,   Framework.net48         },
            {"v4.8"             ,   Framework.net48         }
        };
    }
}
