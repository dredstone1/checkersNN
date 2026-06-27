cmake_minimum_required(VERSION 3.28)
set(CMAKE_CUDA_ARCHITECTURES 86)

project(checkersNN LANGUAGES CXX CUDA)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CUDA_STANDARD 17)
set(CMAKE_CUDA_STANDARD_REQUIRED ON)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

enable_language(CUDA)
include(FetchContent)

FetchContent_Declare(SFML
    GIT_REPOSITORY https://github.com/SFML/SFML.git
    GIT_TAG 3.0.1
    GIT_SHALLOW ON
)

FetchContent_Declare(
    NeuralNetwork
    SOURCE_DIR ${CMAKE_CURRENT_SOURCE_DIR}/../NeuralNetwork
)

FetchContent_MakeAvailable(SFML NeuralNetwork)

file(GLOB_RECURSE LIB_SOURCES CONFIGURE_DEPENDS
    "${CMAKE_CURRENT_SOURCE_DIR}/src/*.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/*.cu"
)

list(FILTER LIB_SOURCES EXCLUDE REGEX ".*main.cpp$")

add_library(checkers_nn STATIC ${LIB_SOURCES})
set_target_properties(checkers_nn PROPERTIES POSITION_INDEPENDENT_CODE ON)

set_target_properties(NeuralNetwork PROPERTIES
    CUDA_SEPARABLE_COMPILATION ON
      CUDA_RESOLVE_DEVICE_SYMBOLS ON
)

target_include_directories(checkers_nn
    PUBLIC
        ${CMAKE_CURRENT_SOURCE_DIR}/include
)

find_package(CUDAToolkit REQUIRED)

target_link_libraries(checkers_nn
    PUBLIC
        sfml-graphics
        sfml-window
        sfml-system
        NeuralNetwork
        CUDA::cudart
)

add_executable(checkersNN src/checkersNN.cpp)

target_link_libraries(checkersNN
    PRIVATE
        checkers_nn
        sfml-graphics
        sfml-window
        sfml-system
        CUDA::cudart
)

message(STATUS "Configured executable")
