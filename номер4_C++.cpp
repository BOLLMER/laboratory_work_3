#include <iostream>
#include <string>
#include <algorithm>
#include <climits>
#include <random>
#include <cmath>

using namespace std;

int main() {
    int n;
    int m;
    vector<int> chisla;

    cout << "Введите N и M через пробел: ";//N (количество чисел) и M (максимальное количество чисел, которые можно взять за ход)
    cin >> n >> m;
    if (n < 0) {
        n = abs(n);
        random_device rd;
        mt19937 gen(rd());
        uniform_int_distribution<> dist(5, 50000);
        for (int i = 0; i < n; i++) {
            chisla.push_back(dist(gen));
        }
        cout << "Случайно сгенерированный список:";
        for (const auto& i : chisla) {
            cout << " " << i;
        }
        cout << "\n";
    } else {
        for (int i = 0; i < n; i++) {
            int z;
            cout << "Введите число №" << i+1 << " > ";
            cin >> z;
            chisla.push_back(z);
        }
    }

    vector<int> dp(n+1);
    dp[n] = 0;
    for (int i = n-1; i >= 0; --i) {
        dp[i] = INT_MIN;
        int sum = 0;
        for (int k = 1; k <= m && i + k <= n; ++k) {
            sum += chisla[i + k - 1];
            dp[i] = max(dp[i], sum - dp[i + k]);
        }
    }

    cout << (dp[0] > 0 ? 1 : 0) << endl;

}
