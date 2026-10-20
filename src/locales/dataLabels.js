// University data (teaching language, deadline) is entered by admins as free Azerbaijani text, e.g.
// "İngilis dili, Azərbaycan dili" or "30 İyul 2026". These helpers show it in the visitor's language via
// the browser's Intl API; Azerbaijani visitors and unrecognised values keep the original text.

// Site language codes that differ from BCP 47.
const LOCALE = { jp: 'ja', kr: 'ko', dk: 'da', se: 'sv', gr: 'el', cz: 'cs', ua: 'uk', ge: 'ka', am: 'hy', kz: 'kk', no: 'nb' };
export const toLocale = (lang) => {
  const code = String(lang || 'en').split('-')[0].toLowerCase();
  return LOCALE[code] || code;
};

// Azerbaijani (and English) language names → ISO 639-1. Tolerates the typos found in the data.
const LANGUAGE_PATTERNS = [
  [/[İIi]ngilis|english/i, 'en'], // "İ" (U+0130) does not case-fold to "i" in JS regexes
  [/azərbaycan|azerbaijani/i, 'az'],
  [/t[üu]r?k\b|türk|turkish/i, 'tr'],
  [/alman|german/i, 'de'],
  [/gürcü|georgian/i, 'ka'],
  [/ərəb|arabic/i, 'ar'],
  [/\brus\b|russian/i, 'ru'],
  [/[İIi]talyan|italian/i, 'it'],
  [/çin|chinese/i, 'zh'],
  [/fransız|french/i, 'fr'],
  [/ispan|spanish/i, 'es'],
  [/yapon|japanese/i, 'ja'],
  [/koreya|korean/i, 'ko'],
  [/macar|hungarian/i, 'hu'],
  [/çex|czech/i, 'cs'],
  [/polyak|polish/i, 'pl'],
  [/holland|niderland|dutch/i, 'nl'],
  [/isveç|swedish/i, 'sv'],
];

const capitalize = (text, locale) => (text ? text.charAt(0).toLocaleUpperCase(locale) + text.slice(1) : text);

/** "İngilis dili, Azərbaycan dili" → "English, Azerbaijani" (in the visitor's language). */
export function localizeLanguages(value, lang) {
  if (!value) return value;
  const locale = toLocale(lang);
  if (locale === 'az' || typeof Intl.DisplayNames !== 'function') return value;

  const found = [];
  for (const [pattern, code] of LANGUAGE_PATTERNS) {
    const m = pattern.exec(value);
    if (m && !found.some((f) => f.code === code)) found.push({ code, at: m.index });
  }
  if (!found.length) return value;

  try {
    const names = new Intl.DisplayNames([locale], { type: 'language' });
    // Keep each language's own casing (e.g. lowercase in Russian/French) and capitalise only the start.
    const list = found.sort((a, b) => a.at - b.at).map((f) => names.of(f.code));
    const text = typeof Intl.ListFormat === 'function'
      ? new Intl.ListFormat(locale, { style: 'short', type: 'conjunction' }).format(list)
      : list.join(', ');
    return capitalize(text, locale);
  } catch {
    return value;
  }
}

const AZ_MONTHS = ['yanvar', 'fevral', 'mart', 'aprel', 'may', 'iyun', 'iyul', 'avqust', 'sentyabr', 'oktyabr', 'noyabr', 'dekabr'];

/** "30 İyul 2026" → "July 30, 2026" (in the visitor's language). */
export function localizeDate(value, lang) {
  if (!value) return value;
  const locale = toLocale(lang);
  if (locale === 'az') return value;
  const m = /^\s*(\d{1,2})\s+(\S+)\s+(\d{4})\s*$/.exec(value);
  if (!m) return value;
  const month = AZ_MONTHS.indexOf(m[2].toLocaleLowerCase('az'));
  if (month < 0) return value;
  try {
    const date = new Date(Date.UTC(+m[3], month, +m[1]));
    return new Intl.DateTimeFormat(locale, { day: 'numeric', month: 'long', year: 'numeric', timeZone: 'UTC' }).format(date);
  } catch {
    return value;
  }
}
