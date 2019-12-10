import { VowelCounter } from '../src/vowel-counter';

describe('getCount', () => {
  it('should pass a sample test', () => {
    expect(VowelCounter.getCount('abra   cadabra')).toBe(5);
  });
  it('no vowels', () => {
    expect(VowelCounter.getCount('brcdb')).toBe(0);
  });
});
