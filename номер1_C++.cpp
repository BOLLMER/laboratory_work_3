#include <iostream>
#include <cmath>
#include <iomanip>

using namespace std;

double f(double x) {
    if (-5 <= x && x < -3) {return 1;}
    else if (-3 <= x && x <= -1) {return -sqrt(4-pow(x+1, 2));}
    else if (-1 < x && x < 2) {return -2;}
    else if (2 <= x && x <= 5) {return x-4;}
    return 13;
}

int main() {
    double x_start = -5;
    double x_end = 5;
    double dx = 0.5;

    cout << "|     x    |     y    |" << endl;
    cout << "|----------|----------|" << endl;
    for(double x = x_start; x <= x_end; x += dx) {
        cout << "| " << setw(8) << x << " | " << setw(8) << f(x) << " |" << endl;
    }
}
