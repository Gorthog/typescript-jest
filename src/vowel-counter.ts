export class VowelCounter {
  static getCount(str: string) {
    const list = str.match(/[aeiou]/gi);
    return list?.length ?? 0;
  }
}
