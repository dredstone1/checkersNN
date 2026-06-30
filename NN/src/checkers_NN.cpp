#include "dataBase.hpp"
#include "tensor.hpp"
#include <model.hpp>
#include <stdio.h>
#include "../include/checkers_NN.hpp"

int main() {
	nn::global::Tensor::toGpu();
	nn::model::Model model("../data/checkers_config.json");
	return 0;
}
