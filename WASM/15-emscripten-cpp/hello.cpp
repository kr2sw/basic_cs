#include <emscripten/bind.h>
#include <string>
#include <vector>

using namespace emscripten;

class Calculator {
private:
    int memory;

public:
    Calculator() : memory(0) {}

    void store(int value) { memory = value; }

    int recall() const { return memory; }

    int add(int a, int b) const { return a + b; }

    int multiply(int a, int b) const { return a * b; }

    std::string greet(const std::string& name) const {
        return "Hello, " + name + "! Welcome to WASM!";
    }

    int sum_vector(const std::vector<int>& values) const {
        int total = 0;
        for (int v : values) total += v;
        return total;
    }
};

EMSCRIPTEN_BINDINGS(my_module) {
    class_<Calculator>("Calculator")
        .constructor<>()
        .function("store", &Calculator::store)
        .function("recall", &Calculator::recall)
        .function("add", &Calculator::add)
        .function("multiply", &Calculator::multiply)
        .function("greet", &Calculator::greet)
        .function("sumVector", &Calculator::sum_vector)
        ;

    register_vector<int>("vectorInt");
}
