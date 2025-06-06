﻿using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IWarningsParser
    {
        Warnings Parse(XDocument document);
    }
}
