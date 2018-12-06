using System;
using System.Collections.Generic;
using System.Text;

namespace Tochka.Algoritms
{
    public class LetterFrequency
    {
        public (long allLetter, Dictionary<char,int> letterCounter) Calc(IEnumerable<string> texts)
        {
            long allLetter = 0;
            Dictionary<char, int> letterCounter = new Dictionary<char, int>();

            foreach (var text in texts)
            {
                foreach (var letter in text)
                {
                    if (!char.IsLetter(letter))
                        continue;

                    allLetter++;
                    if (letterCounter.TryGetValue(letter, out var count))
                        letterCounter[letter] = ++count;
                    else
                        letterCounter.Add(letter, 1);
                }
            }

            return (allLetter, letterCounter);
        }
    }
}
