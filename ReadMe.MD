# MNR Tester
With this CLI tool you could easily test your MNR-programs.

## Input file
The first line declares all the registers you are going to use
All the next lines should contain MNR-code, the first line will have index 1 (as in default MNR programs).

### Example of input file: (test.txt actually)
---
8 2 3 0

J(2, 3, 5)

S(3)

S(1)

J(0, 0, 1)

Z(2)

J(0, 1, 10)

S(1)

S(2)

J(0, 0, 6)

T(2, 0)

---

This is code to calculate f(x, y, z) = x - (y + z) with x=8, y=2 and z=3

## Running
You have 2 options: to see only the result or result with all the register at each step. There are commands run and test respectively.

