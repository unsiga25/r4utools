﻿using Montage.RebirthForYou.Tools.CLI.Entities;
using System.Collections.Generic;

namespace Montage.RebirthForYou.Tools.CLI.API
{
    /// <summary>
    /// A model for post-processing a card that has been successfully parsed.
    /// This includes adding an missing details (like images), or corrections.
    /// </summary>
    public interface ICardPostProcessor
    {
        /// <summary>
        /// Indicates the priority of the post-processor.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Indicates if the list of cards exported are compatible for this post-processor in the first place.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public bool IsCompatible(List<R4UCard> cards);

        /// <summary>
        /// Applies post-processing on a card.
        /// </summary>
        /// <param name="original"></param>
        /// <returns>Returns the processed card. Should not return itself to respect atomicity.</returns>
        public IAsyncEnumerable<R4UCard> Process(IAsyncEnumerable<R4UCard> originalCards);
    }
}
