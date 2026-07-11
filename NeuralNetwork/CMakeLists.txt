cmake_minimum_required(VERSION 3.28)
set(CMAKE_CUDA_ARCHITECTURES 86)  # For RTX 3060
project(NeuralNetwork LANGUAGES CXX CUDA)  # Add CUDA here

# ------------------------------------------------------------------
# Configuration
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CUDA_STANDARD 17)                    # Add CUDA standard
set(CMAKE_CUDA_STANDARD_REQUIRED ON)           # Enforce it
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin")

enable_language(CUDA)

# Default to Debug build type
if(NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE Debug CACHE STRING "Build type" FORCE)
endif()

# ------------------------------------------------------------------
# Paths
set(NN_RESOURCE_DIR_PATH "${CMAKE_CURRENT_SOURCE_DIR}/resources/fonts")

# ------------------------------------------------------------------
# Dependencies
include(FetchContent)

FetchContent_Declare(SFML
    GIT_REPOSITORY https://github.com/SFML/SFML.git
    GIT_TAG 3.0.1
    GIT_SHALLOW ON
    SYSTEM
    EXCLUDE_FROM_ALL
)

set(BUILD_SHARED_LIBS OFF CACHE BOOL "" FORCE)
set(CMAKE_POSITION_INDEPENDENT_CODE ON CACHE BOOL "" FORCE)
FetchContent_Declare(nlohmann_json
    GIT_REPOSITORY https://github.com/nlohmann/json.git
    GIT_TAG v3.12.0
    GIT_SHALLOW ON
    SYSTEM
    EXCLUDE_FROM_ALL
)

FetchContent_MakeAvailable(SFML nlohmann_json)

# ------------------------------------------------------------------
# Main library

# Add both C++ and CUDA sources
file(GLOB_RECURSE NN_SOURCES CONFIGURE_DEPENDS
    "${CMAKE_CURRENT_SOURCE_DIR}/src/*.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/*.cu"   # Include CUDA source files
)

add_library(NeuralNetwork SHARED ${NN_SOURCES})
set_target_properties(NeuralNetwork PROPERTIES POSITION_INDEPENDENT_CODE ON)

# Enable separable compilation for CUDA files
set_target_properties(NeuralNetwork PROPERTIES
    CUDA_SEPARABLE_COMPILATION ON
      CUDA_RESOLVE_DEVICE_SYMBOLS ON       # <--- important for static libs
)

target_include_directories(NeuralNetwork
    PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
        $<INSTALL_INTERFACE:include>
)

target_compile_definitions(NeuralNetwork
    PRIVATE "RESOURCE_DIR=\"${NN_RESOURCE_DIR_PATH}\""
)

find_package(CUDAToolkit REQUIRED)

target_link_libraries(NeuralNetwork
    PUBLIC
        SFML::Graphics
        SFML::Window
        SFML::System
        nlohmann_json::nlohmann_json
        CUDA::cudart
)

target_compile_options(NeuralNetwork PRIVATE 
    $<$<COMPILE_LANGUAGE:CXX>:-Wall;-Wextra;-Wpedantic>
)

# ------------------------------------------------------------------
# Install
install(TARGETS NeuralNetwork 
    LIBRARY DESTINATION lib   # Catches .so files (Linux shared)
    ARCHIVE DESTINATION lib   # Catches .a files (Static/Windows import)
    RUNTIME DESTINATION bin   # Catches .dll files (Windows shared)
)
install(DIRECTORY include/ DESTINATION include)
