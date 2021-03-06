﻿using Montage.RebirthForYou.Tools.CLI.Entities;
using System;
using System.Collections.Generic;

namespace Montage.RebirthForYou.Tools.CLI.API
{
    public interface ICardSetParser
    {
        bool IsCompatible(IParseInfo parseInfo);
        IAsyncEnumerable<R4UCard> Parse(string urlOrLocalFile);
    }
}
