// https://www.codewars.com/kata/square-every-digit/train/typescript
export class Kata {
  static squareDigits(num: number) {
    return Number(
      num
        .toString()
        .split('') // convert a number to an array of digits (each digit is a string)
        .map(c => Number(c))
        .map(n => n ** 2)
        .map(n => n.toString())
        .join('') // convert an array of numbers into a string - without a separator
    );
  }
}
