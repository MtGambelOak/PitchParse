
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

Lastly, note that the output is sometimes not always sorted perfectly. For example, words that are neither verbs nor adjectives may be included in `adjectives and verbs.txt`; currently it is not possible to filter these out perfectly.

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

# I-Adjectives and Verbs
Next, let's move onto `adjectives and verbs.txt`. Perhaps unsurprisingly, I-Adjectives are at the top of this file.

Everything in this file is easy because accents on adjectives and verbs will always occur on the second to last mora, if they occur at all. Thus there are essentially only two possibilities we have to consider for each word.
## I-Adjectives
I-Adjectives are even easier; I won't cover their phonetic shift here, but regardless, there are very few accentless I-Adjectives to begin with. These can be memorized very easily with Anki. I might as well even list the only common ones right here:
> ```
>辛い つらい 0 freq 1166
>軽い かるい 0 freq 1284
>重い おもい 0 freq 1520
>薄い うすい 0 freq 1786
>赤い あかい 0 freq 1874
>遠い とおい 0 freq 1878
>固い かたい 0 freq 1944
>暗い くらい 0 freq 2343
>厚い あつい 0 freq 3407
>丸い まるい 0 freq 4190
> ```

## Verbs
**I think verbs are the most important words to learn the pitch accent of. Why?**
Because in my opinion, they're relatively "low hanging fruit" that will have a huge impact on your spoken Japanese:

 - Since they can only have two possible pitch accents, the possibilities you have to consider are less, making them easier to learn. There are about twice as many accented verbs as there are accentless, so memorizing the accentless verbs cuts down on the total by a factor of 3.
 - There are comparatively less verbs below a given frequency cutoff in the output, but they're used almost as often as nouns in actual speech! i.e. only a bit above 1000 nouns appear in the default output, compared to many more in `1-4 mora nouns.txt`.
 - All in all, this means only around 270 words need to be memorized to effectively "learn" the accents of all the verbs, as many can be pronounced both ways, and we don't need to include both transitive and intransitive versions of the same verbs!

 Of course, conjugations can be a beast, so verbs aren't a total walk in the park. It should also be noted that *only* adding a few hundred accentless verbs to an Anki deck and calling it a day doesn't always work the best, as knowing that every verb is accentless within your reps means they aren't the most effective. I think it could even make sense to add an equal amount of downstep verbs to a deck; while this results in more cards, it definitely ensures you master the accents of verbs, which is super important.

## Compound Verbs
The vast majority of compound verbs are downstep verbs. As a matter of fact, there's so few exceptions, I might as well include all the reasonably common ones here:
> ```
>見付ける みつける 0 freq 982
>出掛ける でかける 0 freq 1493
>落ち着く おちつく 0 freq 1737
>取り替える とりかえる 0 freq 7374
>引き摺る ひきずる 0 freq 9112
>見張る みはる 0 freq 11119
>気取る きどる 0 freq 14473
>真似る まねる 0 freq 14889
> ```
