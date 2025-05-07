
# Tutorial
This section is not intended to be a tutorial of how to get the project set up and running. Consult the [Quickstart Tutorial](README.md#quickstart-tutorial) to get things started.
**Note that I am not a pitch accent expert! Many statements I got from sources such as the NHK Pitch Accent Dictionary or otherwhere on the internet, or are quantitative patterns I am seeing in the data. I am not guaranteed to be remembering or communicating information perfectly accurately**

Rather, this is going to be more of a tutorial of how I suggest the project is used, i.e. how to actually learn pitch accents with PitchParse.

If you haven't read about the data used in the project, do so now! [About the data](about_data.md)

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
This section basically refers to nouns that have longer character and mora counts.  In the output, these are mostly compound nouns that are 3 characters long, such as words like `必要性`.

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

Perhaps a better name for this section would be phrasal nouns (I am not sure if they have an official name). Simply put, certain nouns can have multiple pitch accents at different locations within the "word", as if they were made up of two separate words part of the same phrase. More can be found on the Wikipedia page on Pitch Accent, in the section "Compoundified compound nouns vs noncompoundified compound nouns".

Lines are displayed as such:

`万が一 まんが・いち 1・1 freq 6138`

The boundary between the two words is shown by the `・` symbol in the reading. The two accents then, show where the downstep is in the respective words.

For whatever reason, most of the frequency data does not contain these words very often (perhaps because they are parsed into individual words). Changing the setting `Get all words regardless of frequency or appearance in deck?` to `Y` is a way to get all of them to display, but there are a lot.

Unfortunately, I do not know of any way to tell if a word is going to be a "regular" compound noun with one accent, or a noun like this with multiple! Even yojijukugo can be part of either category, and long compound nouns like `第一次世界大戦` can also be of either category. This is yet another thing to just be aware of and watch out for, but I wouldn't let it keep you up at night.



# 1-4 Mora Nouns
This section will hopefully me more straightforward; but `1-4 mora nouns.txt` is by far the largest output file, so there are a lot more words to worry about!
## 4 Mora Nouns
For the most part, the majority of 4 mora nouns are going to be accentless. This pattern is stronger or weaker for certain classes of words; for example, one kanji nouns of Japanese origin such as `湖` often are *not* accentless, but there aren't very many of these words. But for the most part, this pattern does hold; around 80% of 4 mora nouns in the default output are accentless. Thus, memorizing the ones that aren't will be the most effective, though it should be noted there are still a fair amount of exceptions, as there are simply a *lot* of 4 mora nouns in Japanese! About half of the exceptions have an accent on the first mora, and the rest are mostly equally split between having a downstep on the second or third mora. *Very* few four mora nouns are `尾高`, with `妹` and `弟` being the only real exceptions.
## 3 Mora Nouns
Unfortunately, there is no overwhelming pattern that makes 3 mora nouns easy to learn.
Very few have downsteps on the last or second to last mora; A bit over half are accentless, and around 40% have a downstep on the first mora, leaving less than 10% being `中高` and `尾高`. Memorizing these outliers means anything else is going to be `平板` or `頭高`, mostly just up to a guess with one being a bit more likely! I think it may make sense to still memorize the most common 3 mora words that are of these two categories, but getting them all is a lot. I might advise taking these on a word by word basis after picking off the "low hanging fruit".
## 2 Mora Nouns
There are only 3 possible accents a 2 mora noun can have. For the most part, they tend to have a downstep on the first mora, and are either accentless or have an accent on the last mora with about equal frequency. This tendency is even stronger if the word has 2 kanji; these words are almost always `頭高`. I would at least memorize the exceptions for 2 kanji words here, as there aren't as many. As for the rest, it's up to you; there are a lot to memorize.
## 1 Mora Nouns
There are not very many of these, and they can only have two possible accents. Memorizing the few accentless ones should be easy enough.

# Hiragana & Katakana words
`hiragana and katakana words.txt` contains exactly what you would expect.
## Hiragana Words
These are mostly just sorted together as there isn't necessarily another place to put them. However, there are a few patterns here to take into account.
Specifically, [Onomatopoeia (擬音語) and Mimetic Words (擬態語)](https://onomatoproject.com/list.html) (which often are written in Hiragana only) seem to have patterns (that I've noticed, I have no official source for these being hard and fast rules).

 - Reduplication words like `わくわく` have a downstep on the first mora. This sometimes depends on if they are being used as adverbs or adjectevial nouns: take `ふわふわ  (副)1,(形動)0` for example.
 - I'm not sure if there is a term for these types of words, so I'll call them `り` onomatopoeia. These words always end with `り`, and if the word is four mora long, have a `っ` or `ん` for the second mora. Examples include `ばっちり`, `ぼんやり`, `にこり`.  The downstep is always on the second to last mora, but can also be on the last mora if it is a 3 mora long word. Words like `あんまり` may appear to be an exception, but I don't believe this word is an Onomatopoeia or Mimetic, so I'm not sure if this counts.

There are likely even more patterns, as I have noticed something with `っと` words. But I'm not as confident these patterns exist, so I won't talk about them in depth here.

A good amount of the outputted hiragana words fall under these categories, so all the ones that are not exceptions to these patterns can be ignored. Many remaining words are just hiragana only versions of other words that are included in the frequency/accent data for whatever reason, meaning those don't really need to be worried about here as well. In the end, mostly a few odd grammatical and miscellaneous words remain - I have not seen any patterns here, but there's few enough to make memorizing them easy.

## Katakana Words
This section might as well have been callen loanwords, as well, the vast majority of katakana words are just loanwords. In any case, there's mostly one thing to look out for in these words; most follow the "-3 rule", dictating the downstep is on the third to last mora, if there is one at all. Quite often, the third to last mora is a "special sound" that can't hold the accent however, so it's often pushed one more forward in the word making it even earlier! Words in the output that violate this rule should be tagged:

`デジタル  1 irregular downstep  freq 2197`
There are a lot of loanwords however, and so there are still a lot of exceptions. Really, it just depends on what words you think you're ever going to use in a conversation, and how often you want to use loanwords in the first place. Many of these words, even ones  reported as occurring somewhat frequently such as `フェスティバル`, have natural Japanese equivalents, and I'm a bit surprised the frequency data seems to favor so many of these words, as I don't recall hearing them often.

There are comparatively few accentless words here, I think the important ones can be manually memorized fairly straightforwardly.

Lastly, note that some words in the output will be flagged with `irregular ー pair`.
This has to do with the fact that Dogen states many 3 mora katakana words ending with a long vowel have an accent on the second mora - but I found this to not often really be the case. I believe he may have meant that these kind of words are more likely to violate the -3 rule, rather than these kind of words *usually* being pronounced such a way. I included it just in case, but I would just ignore these flags.


# "Stem Nouns"
`stem nouns.txt` refers to nouns that originate from verb "stems" as I have always called them (I belive this is what Tae Kim calls the `~ます` conjugation for verbs). But I think the proper term  would be to call these "verbal nouns".

For example, take `育ち` deriving from `育つ`. My source for this section is some Reddit comment I saw forever ago and can't find anymore, but this does seem to be an actual pattern:

If the original verb is accentless, the resulting noun is accentless. If the original verb is accented, the resulting noun has a downstep on the last mora, resulting in a `尾高` word. If the original verb was a compound verb, the resulting noun is often accentless regardless! (sometimes the output incorrectly flags these as exceptions, ignore that pls)

The output of this file is a little unorganized and incorrectly picks up on some words that are not actually originating from a verb. Additionally, it can sometimes be difficult to actually remember and apply this rule; it predicates you know the accent of the original verb (which is sometimes much rarer than the noun itself), and some words are often not written with okurigana and as such I never even particularly thought of them as these types of words (ex `光`). But, some may find it useful. (This file can be toggled off in the settings!)



# Conclusion and Final Cheatsheet

A lot of patterns exist that we can exploit to learn pitch accent in a "more efficient" manner; following the guidelines in this tutorial, it's likely possible to create an Anki deck with ~1000 cards that will allow someone to still speak with very accurate pitch accent:

 - Common verbs in ~300 cards
 - Various misc nouns (hiragana and katakana words, suffixes, etc) in ~300 cards
 - I-Adjectives in ~15 cards
 - ~200 4 mora nouns 
 - ~150 3 mora nouns
 - ~100 2 mora nouns
 - ~20 1 mora nouns

This would get the vast majority of "common" (below the frequency cutoff) verbs, compound nouns, hiragana nouns, loanwords, I-Adjectives, and 1 mora nouns, assuming the exceptions are remembered correctly.  Many exceptions for 4 mora nouns would "slip through the cracks", but the most important ones would be remembered, and simply "guessing" `平板` gives over 80% accuracy! The same goes for 3 and 2 mora nouns, though "guessing" is perhaps not as accurate for those. Basically, the only words you wouldn't know the pitch accent of are less common exception words that are 4-2 mora nouns, which I feel is not too bad for only having to go through 1000 cards! And adding another 1000 would greatly increase the accuracy on the weak spots.

Reminder: I made some [example decks](https://ankiweb.net/shared/by-author/1801041122) shared on AnkiWeb!

However, this is of course an idealized scenario. It's likely exceptions will not be remembered perfectly. In addition, only repping exceptions on Anki can cause a bad habit of "hallucinating" exceptions, since you're seeing them so often! It should hopefully be obvious that this approach alone will not give you perfect pitch accent. Heavy immersion, perhaps assisted by this approach, is a much better way to go.

## Accent Patterns Cheatsheet

 - Around 2/3 of verbs are downstep verbs
 - The vast majority of compound verbs are downstep verbs
 - The vast majority of I-Adjectives are downstep adjectives
 - The accent of compound nouns depends on the suffix. Most suffixes result in a word with a downstep somewhere in the middle of the word (near where the suffix connects), but a few result in an accentless word
 - The morphology of compound nouns is important in determining the suffix and therefore the accent. Most compound nouns longer than 3 kanji/morphemes will have a downstep somewhere around the middle
 - Some "compound nouns" actually have multiple downsteps as if they were individual words part of a phrase!
 - The vast majority of 4 mora nouns are accentless
 - The vast majority of 3 mora nouns have either no downstep, or a downstep on the first mora
 - Most 2 mora nouns have a downstep on the first mora, especially if they have 2 kanji
 - Most 1 mora nouns have a downstep after their lone mora
