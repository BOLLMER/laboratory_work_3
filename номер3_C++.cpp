#include <iostream>
#include <cmath>
#include <string>
#include <numeric>

using namespace std;

string sokr(int chisl, int znam) {
    int g = gcd(chisl, znam);
    return to_string(chisl / g) + "/" + to_string(znam / g);
}

string calc_sum(int a, int b) {
    if (b == 1) return "infinity";

    if (a == 1) {
        int znam = (b - 1) * (b - 1);
        return sokr(b, znam);
    }

    if (a == 2) {
        int chisl = b * (b + 1);
        int znam = (b - 1) * (b - 1) * (b - 1);
        return sokr(chisl, znam);
    }

    if (a == 3) {
        int num = b * (b*b + 4*b + 1);
        int den = (b-1)*(b-1)*(b-1)*(b-1);
        return sokr(num, den);
    }


    double sum = 0;
    int max_terms = 10000;
    double prev_sum = 0;
    bool converged = false;

    for (int n = 1; n <= max_terms; n++) {//Вычисление суммы с проверкой сходимости2
        sum += pow(n, a) / pow(b, n);
        if (fabs(sum - prev_sum) < 1e-10) {
            converged = true;
            break;
        }
        prev_sum = sum;
    }

    if (!converged) {return "irrational";}

    if (fabs(sum - round(sum)) < 1e-10) {
        return to_string(static_cast<int>(round(sum))) + "/1";
    }

    for (int denom = 2; denom <= 1000; ++denom) {
        double numerator = sum * denom;
        if (fabs(numerator - round(numerator)) < 1e-10) {
            return sokr(static_cast<int>(round(numerator)), denom);
        }
    }
    return "irrational";
}

int main() {
    int a, b;
    cout << "Введите значения a и b через пробел: ";
    while (true) {
        cin >> a >> b;
        if (a < 1 || a > 10 || b < 1 || b > 10) {
            cout << "Числа должны быть в диапазоне от 1 до 10!" << endl;
        } else {break;}
    }
    cout << "Результат: " << calc_sum(a, b) << endl;
}
