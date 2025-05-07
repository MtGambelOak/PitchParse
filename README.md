# PitchParse

PitchParse is a relatively simple console application that utilizes available pitch accent and frequency data to streamline the process of learning the pitch accent for words.
It also identifies minimal pairs and homophones in regards to pitch accent.
Words are sorted into output files based on various factors. Words can be sorted/filtered based on their frequency/occurence within a user's Anki collection.

While certainly not a silver bullet in regards to pitch accent, it is designed to save time memorizing words.
As always, a tool like this should be supplemented with heavy immersion to see any real effects.


An excerpt from the *NHK 日本語発音アクセント新辞典, (付録 p. 14)*:

> 日本語を話す人は、それぞれの単語のアクセントを、なんの手がかりもなく一語一語すべてゼロから記憶しているわけではない。  それがどんなことばであるかによってある程度の「傾向」があるものがあり、それを無意識に使用することによって、記憶量の節約を図っていると考えられる。  そして「傾向」に合わないことばを、「例外」として一語一語覚えているのである。  
> つまり、こうした「傾向」を頭に入れておくと、「例外」だけを覚えていけばいいので、アクセントを習得するのが楽になる。

There are 3 things that make it worth learning the pitch accent of a word:
1. It should be common in everyday speech
2. It should be an exception to a common pattern/tendency
3. If it is part of a minimal pair (or triplet etc) in regards to its pitch accent

# About the Data
The data used in this project is important to discuss, more can be found [here](about_data.md).

# Tutorial

Follow this for a full guide as to how to actually interpret and use the output: [Full Tutorial](tutorial.md)

# Example Decks

I made some example decks (some are quite complete!) using this tool! Shared on [Anki Web](https://ankiweb.net/shared/by-author/1801041122).

# Quickstart Tutorial

1. Download PitchParse from the [releases](https://github.com/MtGambelOak/PitchParse/releases) section and unzip the file.
2. Choose what pitch accent data file you would like to use. By default, `combinedAccents.txt` is already present in the inputs folder, and this is the reccomended data file as it's a combination of the most important data from four separate sources and gets the widest coverage. Swap it out with another from the [Accents folder](https://github.com/MtGambelOak/PitchParse/tree/master/Accents) if you want to use another data file.
> (optional) if planning to make Anki cards with the help of PitchParse, it is highly reccomended that you download the `combined_pitch_accents.zip` dictionary and install it to Yomitan so Anki cards have consistent accent data.
3. Choose what frequency data files you would like to use. By default, 5 frequency data files are present in the frequency folder and are the 5 that contain reading data. Use any combination you like from the [Frequencies folder](https://github.com/MtGambelOak/PitchParse/tree/master/Frequencies), just know that ones without reading data will not work as well and result in slightly more cluttered output. Frequency data files were modified from ones available in [MarvNC's Yomichan dictionaries repository](https://github.com/MarvNC/yomitan-dictionaries), more data on them can be found there.
4. (optional) Get data from your Anki collection to filter outputs to only include words that appear in your collection. Download [this anki addon](https://ankiweb.net/shared/info/1758053224) and ouput all cards you want to scan to text and make sure the file is named `cards.txt`. In the settings file, make sure you enter which fields you want to scan with a space after the colon and separated by commas (no spaces). EXAMPLE:
> ![image](https://github.com/user-attachments/assets/dab2f4ec-3a1f-42e7-9528-9cdf4ff0ad9e)
5. Change the settings to your liking. By default the settings in `settings.txt` are set to the "reccomended" defaults. Full explanation of the settings can be found below or in the tutorial video. (Make sure you toggle the first two frequency settings off if you are reading from an Anki collection)
6. Run the program by double clicking the executable file PitchParse.exe. Outputs will be located in the outputs folder. Note that if running with Anki collection for the first time, finding words you know will take some time. After the first time running this information is remembered so future executions will be much much faster (a few seconds)

# Settings

PitchParse has multiple settings to change how the program functions contained in the `settings.txt` file. Keep in mind the formatting for this file is a little specific, so don't mess with it too much or the program may fail to work. To turn settings on make sure the last character on the line is a case sensitive `Y`. For settings that control numbers make sure the number is the last thing on the line.

*Filter previously learned words from old files?*
*Write newly learned/non filtered words to old files?*
These two settings are intended to filter stuff already learned in a previous session.
Example: I use PitchParse today and one of the words I learn is `公開`. 6 months pass. I've learned a lot of new words and may want to make sure I know their pitch accents (if I wasn't paying attention while learning them) so before updating my `cards.txt` I run the program with the write setting turned on. Now after updating `cards.txt` and 	`words.txt` (will need to delete words so it's regenerated), words like `公開` are filtered if the filter setting is turned on. If a new word that I learned is `後悔` these will still show up as minimal pairs, because this is a new minimal pair.
These settings may not be *too* useful and also I haven't tested them extensively, they could potentially be convenient for some though.

*Sort nouns by secondary pitch?*
The way data is formatted, words can have multiple pitch accent values.
Example: `建物	たてもの	2,3`
The ordering is important though - the first accent is the more common one, but both are accepted. This setting determines whether words will be sorted depending on the second value rather than just the primary pitch and type of word.

Sort non-mora sorted categories (i.e. stems, compounds, verbs) by mora count, and mora sorted categories by char count?
Words not sorted by mora count (so pretty much everything other than 1-4 mora count nouns) may want to be sorted by mora count for whatever reason, this setting toggles this. So in verbs output all 2 mora verbs would come before 3 mora verbs, etc.
For nouns, their kanji count can sometimes help determine their pitch accent (in terms of patterns). This setting will sort categories such as 2 mora nouns by character count first, so words like `足` appear before words like `部屋`.

*Lump together kifuku words?*
When it comes to categories of words like verbs and adjectives, all words with an accent have it on their second to last mora. Thus they are often sorted into two categories of "downstep" or "kifuku" and "accentless", this setting means all kifuku words will be sorted as if they had the same pitch/are part of the same category.

*Flag exceptions in output?*
The basic concept of PitchParse is to identify any patterns within Japanese pitch accent and memorize the outliers. This setting will flag outliers (via a text description) in the output. For example, if most `ぶ` verbs are unaccented, accented `ぶ` verbs will be flagged.

*Display minimal pairs/homophones in Anki mode?*
This setting displays the output in a format that makes adding the cards to Anki much simpler and faster.


*Filter by frequency only?*
For users not wanting to filter output via Anki (don't have a collection etc) this setting should be turned on. It means all words (in the accents data file below the frequency cutoff) will be output.

*Get all words regardless of frequency or appearance in deck?*
This setting will simply output every word contained in the accents data file will be output regardless of presence/frequency in the frequency data files. Frequency for words not present in the frequency data will be set to 1 above the frequency cutoff setting. This setting is not really reccomended other than people looking out of curiousity as the output will be very cluttered.

*Show frequency values in output?*
This setting simply determines whether the frequency data of a word will be displayed or hidden in the output.

*Differentiate frequencies by readings?:*
This setting determines whether readings will play a role in mapping words to frequencies. For example: `accents.txt` contains an entry for both `目標(もくひょう)` AND `目標(めじるし)`. The first reading is much more common and really the only one we want to concern ourselves with, so this setting means both have the frequency data reported accurately with respect to reading and the latter will almost always be filtered. Note that half of the frequency data files do not have reading data so this setting doesn't work if these files are being used.

*Max frequency value in output? Defaults to 30000:*
Sets the frequency value above which words will not be included in the output. Note that due to the way frequency data was gotten, a value of 30,000 does not mean that the most common 30,000 words will be gotten; the true number of words is likely much less.


*Find all "possible" minimal pairs? Also makes homophone requirements stricter:*
This setting will consider words that *could* technically be minimal pairs (like `騎士,棋士`) both with accent values of `1,2` as minimal pairs. Basically even if words have the same primary pitch, if another one has another value they will be considered minimal pairs. This setting is mostly useful to make the homophone output more "clean" since it also ensures everything in the homophones output are words that are one accent value and one only.


*Isolate verb stems?*
This setting isolates "verb stems", more accurately, the `連用形` form of verbs that function as nouns. This is because these words exhibit *some* patterns respective to the verb they originate from but there are a lot of exceptions and I haven't found any official sources that talk about this. This setting may work better off.

*Include 3 character nouns in 3/4 mora nouns output?*
This setting will redirect 3/4 mora nouns that are 3 or more characters, simply because many of these words are really compounds/function differently than most 3/4 mora words. They are redirected to `miscellaneous nouns.txt`

*What endings to filter long/misc nouns with?:*
This setting filters long/misc nouns (often compounds) that end with specific endings, because many common endings such as `的(てき)` always make words accentless and as such don't need to be memorized (provided the ending is memorized). Note this setting only filters words with the ending and reading that are also heiban. Format by starting with a space after the colon, then the endings kanji, then a comma, then the reading, etc. Regular commas and Japanese commas `、` should both work.

*Find all i-adjectives?*
Since there are actually not a lot of i-adjectives (and even less heiban ones) this setting just outputs all of them if you want to learn them all.

*Display つ verbs in verbs output?*
All `つ` verbs are downstep verbs, so they aren't output (in the verb output) by default. This setting changes that, but since they're all downstep, there isn't much point.
