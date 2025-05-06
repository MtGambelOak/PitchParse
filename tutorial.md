
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


# Compound, Longer, & Miscellaneous Nouns
Unfortunately, it's not exactly possible to filter out all compound nouns and separate them from other types of words. But currently, they can be identified semi-reliably.

`long and compound nouns.txt` contains three sections. We will go through them individually

## "Long Nouns"
This section basically refers to nouns that have longer character and morae counts.  In the output, these are mostly compound nouns that are 3 characters long, such as words like `必要性`.

When I was originally learning pitch accent from Dogen's patreon course, the advice he gives is to just assume all compound nouns are `中高`, with the downstep somewhere on the boundary between the two words getting combined. This advice is great and mostly true, but perhaps overly simplistic. It's been a while since I've read the NHK Accent Dictionary, so I may be misremembering some details here, but I believe they explain compound nouns as having pitch dependent on the suffix in the combination. They outline four different types of suffixes, and how they behave in regards to pitch:

 - Downstep is in the same location of the suffix as a standalone word
 - Downstep is right before the boundary
 - Downstep is right after the boundary
 - The resulting word has no downstep

The first category I found to be quite rare. Furthermore, the first 3 will all result in `中高` compounds (with 2 and 3 only having a downstep difference by 1 mora!). I think it by far makes the most sense to memorize the suffixes that result in accentless compounds, and then treat everything else as having a downstep "somewhere around" the boundary of the two words rather than splitting hairs over the countless suffixes that exist. I made a list of all of these suffixes, and a corresponding Anki deck as well using example words with the suffixes.

So just memorize this list and we're done? Alas, life is never that simple. There are two (perhaps major) caveats I must mention:
1. Many suffixes can be either "accentless" suffixes or "downstep" suffixes. They can be one or the other arbitrarily, or depending on the word they are attaching to, or the specific meaning they are adding as a suffix. I think there are too many of these to necessarily worry about, so I only included suffixes that **always** result in accentless compounds in the aforementioned list. If you do want to worry about this, check out the NHK Accent Dictionary.
2. The morphology of the compound nouns determines what the suffix is! For example, take the word `新幹線`. The suffix `線` typically makes compounds accentless as a suffix, so why is the resulting word not accentless? The reason why is because the word is the result of `新`+`幹線`, NOT `新幹`+`線` - so the suffix actually `幹線`, and full on words like this are not really ever going to be accentless suffixes. So even words that seem to end with an accentless suffix may not actually be accentless, depending on the morphology of the word!

The second caveat seems like a horribly complicated exception to always keep in mind, and in some senses, it definitely is. But I think in practice, not only does it get easier with practice, but it actually ends up being very helpful! Longer compound nouns like `白人男性` almost always have a full on word such as `男性` as the suffix, so you already know the resulting word will have a downstep near the boundary, without needing to know anything about the last character in the word.

## "Miscellaneous Nouns"
This part of the file basically includes all nouns that were not fit into any other category. 

As such, it aptly pretty much just contains lots of random words.

Naturally, there are not many patterns as a result. Do whatever you wish with these words, I mostly just memorized whichever ones I thought were interesting from here.

## "Multiple Word Nouns"
Oh, you thought we were done with the complicated stuff after talking about *regular* compound nouns?

Perhaps a better name for this section would be phrasal nouns. Simply put, certain nouns can have multiple pitch accents at different locations within the "word", as if they were made up of two separate words part of the same phrase. More can be found on the Wikipedia page on Pitch Accent, in the section "Compoundified compound nouns vs noncompoundified compound nouns".

Lines are displayed as such:

`万が一 まんが・いち 1・1 freq 6138`

The boundary between the two words is shown by the `・` symbol in the reading. The two accents then, show where the downstep is in the respective words.

For whatever reason, most of the frequency data does not contain these words very often (perhaps because they are parsed into individual words). Changing the setting `Get all words regardless of frequency or appearance in deck?` to `Y` is a way to get all of them to display, but there are a lot.

Unfortunately, I do not know of any way to tell if a word is going to be a "regular" compound noun with one accent, or a noun like this with multiple! Even yojijukugo can be part of either category, and long compound nouns like `第一次世界大戦` can also be of either category. This is yet another thing to just be aware of and watch out for, but I wouldn't let it keep you up at night.
