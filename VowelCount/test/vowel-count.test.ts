import { Kata } from '../src/vowel-count';

describe('getCount', () => {
  it('should pass a sample test', () => {
    expect(Kata.getCount('abra   cadabra')).toBe(5);
  });
  it('no vowels', () => {
    expect(Kata.getCount('brcdb')).toBe(0);
  });
});
