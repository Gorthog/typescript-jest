import { Kata } from '../src/square-digit';

describe('squareDigits', () => {
  it('should pass a sample test', () => {
    expect(Kata.squareDigits(9119)).toBe(811181);
  });
});
