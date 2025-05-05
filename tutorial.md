
# Tutorial
This section is not intended to be a tutorial of how to get the project set up and running. Consult the [Quickstart Tutorial](README.md#quickstart-tutorial) to get things started.

Rather, this is going to be more of a tutorial of how I suggest the project is used, i.e. how to actually learn pitch accents with PitchParse.

# About the output
Each line in the output will typically be a word, followed by its reading, its accent, and frequency information.

Pitch accent data is displayed in the format of numbers. The number includes which mora the accent occurs on. If there are multiple numbers separated by commas, the different numbers represent multiple correct accent locations. The first is typically considered to be "more common", but keep in mind, **both are correct! I strongly recommend going with any "correct" pronunciation that is easiest to remember.**

Frequency information is specified such that lower numbers represent a higher frequency. But please note that a frequency value of 2000 does not necessarily represent the "2000th" most common word, and filtering with a cutoff of 15000 won't get exactly 15000 words.

Example:

`玄関 げんかん 1 freq 3143`

Some words have different pitch accent based on their part of speech. The part of speech will be displayed in parentheses right before the accent number:

`一段 いちだん (名)2,(副)0 freq 10085`

Lastly, a few words have different pitch accent based on what meaning they have (when multiple exist). Example usages for the different meanings, and their respective pitches, are likewise displayed in parentheses right before the accent, and taken straight from NHK:

`背 せ (～が高い)1,(～を向ける)0 freq 3507`

# Minimal Pairs
At first glance, `minimal pairs.txt` may appear to be the "most important" file. After all, minimal pairs may cause confusion when pronounced with incorrect pitch; at least, this is what people often say. In reality, I think it's important to consider this quote from the Wikipedia page for Pitch Accent ([with this source](https://en.wikipedia.org/wiki/Japanese_pitch_accent#cite_note-34)):
> In 2014, a study recording the electrical activity of the brain showed that native Japanese speakers mainly use context, rather than pitch accent information, to contrast between words that differ only in pitch.

Specifically, page 36 of this paper states:
> ...pitch-accent is not a critical factor in Japanese sentence comprehension

I take this to mean that even if you mix up the accents of everyday words, a Japanese person is still never going to falsely think that you walked across some chopsticks on the way to the park today. (which should hopefully be obvious). You might just sound a little funny, so minimal pairs may still be important to pay attention to, just without considering them as a life-or-death situation.

The output of `minimal pairs.txt` will contain many lines looking like this:

`医師 1 意志 1 意思 1 いし 石 2`

Technically, many are minimal triplets, or several homophones each part of a minimal pair, like the above line. Words are simply listed one after the other, with their accent following. The reading of the words will be located at an arbitrary location in the line.

If wanting to put this information into Anki, I would recommend toggling the setting `Display minimal pairs/homophones in Anki mode?` to `Y`, and rerunning the executable, which causes the output (for the previous example) to be displayed as such:

`医師、意志、意思
(石 2)。`

`石
(医師 1 意志 1 意思 1)。`

Where each possible "group" of pronunciations gets a line, with its minimal pairs below it and its homophones next to it.

The Anki format I like to use is have audio on the front, and the task is to remember what word(s) are pronounced like the audio you heard, and which are minimal pairs in regards to the pitch of the audio. Thus, I usually use `{audio}` for the front and `{expression}` for the back when creating with Yomitan.

# Homophones
The `homophones.txt` file may not contain super useful information for *speaking* with better pitch accent. But I included the functionality anyways, as some may find it useful. I think if a person hasn't gotten a lot of listening immersion, identifying homophones might be useful in disambiguating possible words when hearing something they didn't quite catch, however. Lines will simply contain a reading, and many words with identical pronunciations and pitch accents:

`かてい 仮定 家庭 過程 課程 0 `

Turning on Anki mode in the settings just like in the previous section, the output it more streamlined, with each line formatted like

`仮定、家庭、過程、課程 0。`

Making it easier to add each group as a sentence to Yomitan.


