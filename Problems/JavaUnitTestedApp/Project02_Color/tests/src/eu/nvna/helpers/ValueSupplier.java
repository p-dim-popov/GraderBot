package eu.nvna.helpers;

import org.junit.jupiter.params.provider.Arguments;

import java.util.stream.Stream;

public interface ValueSupplier {
    public static Stream<Arguments> supplyColors() {
        return Stream.of(
                Arguments.of((short)0, (short)0,(short) 0),
                Arguments.of((short)255, (short)124, (short)26),
                Arguments.of((short)0, (short)12, (short)0),
                Arguments.of((short)0, (short)12, (short)255)
        );
    }
}
