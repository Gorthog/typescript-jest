module.exports = {
  preset: 'ts-jest',
  testMatch: ['**/test/**/*.test.ts'],
  collectCoverage: true,
  coverageReporters: ['text', 'html', 'lcov'],
  collectCoverageFrom: ['src/**/*.ts'],
};
