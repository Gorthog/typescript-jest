import { Kata } from '../src';

describe('getCount', () => {
  it('should pass a sample test', () => {
    expect(Kata.getCount('abracadabra')).toBe(5);
  });
});
