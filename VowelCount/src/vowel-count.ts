// https://www.codewars.com/kata/vowel-count/train/typescript

export class Kata {
  static getCount(str: string) {
    const list = str.match(/[aeiou]/gi);
    return list?.length ?? 0;
  }
}
